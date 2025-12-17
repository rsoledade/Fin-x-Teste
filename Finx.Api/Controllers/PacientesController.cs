using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Finx.Api.DTOs;
using Finx.Api.Handlers;
using Microsoft.AspNetCore.Authorization;

namespace Finx.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PacientesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PacientesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Create([FromBody] CreatePacienteDto dto)
        {
            var command = new CreatePacienteCommand(dto.Nome, dto.Cpf, dto.DataNascimento, dto.Contato);
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var pacientes = await _mediator.Send(new GetPacientesQuery(page, pageSize));
            return Ok(pacientes);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var paciente = await _mediator.Send(new GetPacienteByIdQuery(id));
            if (paciente == null) return NotFound();
            return Ok(paciente);
        }

        [HttpGet("{id}/historico")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult GetHistorico(Guid id)
        {
            // placeholder for historico endpoints (to be implemented)
            return Ok(Array.Empty<object>());
        }
    }
}