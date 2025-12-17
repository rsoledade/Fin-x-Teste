using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Finx.Domain;
using Finx.Api.DTOs;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System;

namespace Finx.Api.Handlers
{
    public class GetPacientesHandler : IRequestHandler<GetPacientesQuery, IEnumerable<PacienteDto>>
    {
        private readonly IPacienteRepository _repo;
        private readonly IDistributedCache? _cache;

        public GetPacientesHandler(IPacienteRepository repo, IDistributedCache? cache = null)
        {
            _repo = repo;
            _cache = cache;
        }

        public async Task<IEnumerable<PacienteDto>> Handle(GetPacientesQuery request, CancellationToken cancellationToken)
        {
            int page = request.Page;
            int pageSize = request.PageSize;

            string cacheKey = $"pacientes:page:{page}:size:{pageSize}";
            if (_cache != null)
            {
                var cached = await _cache.GetStringAsync(cacheKey, cancellationToken);
                if (!string.IsNullOrEmpty(cached))
                {
                    return JsonSerializer.Deserialize<IEnumerable<PacienteDto>>(cached) ?? Array.Empty<PacienteDto>();
                }
            }

            var list = await _repo.ListAsync(page, pageSize);
            var dto = list.Select(p => new PacienteDto
            {
                Id = p.Id,
                Nome = p.Nome,
                Cpf = p.Cpf,
                DataNascimento = p.DataNascimento,
                DataCadastro = p.DataCadastro,
                Contato = p.Contato
            }).ToList();

            if (_cache != null)
            {
                var opts = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                };
                await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(dto), opts, cancellationToken);
            }

            return dto;
        }
    }
}
