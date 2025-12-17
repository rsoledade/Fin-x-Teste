using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Finx.Api.DTOs;
using Finx.Api.Handlers;

namespace Finx.Api.Controllers
{
    [ApiController]
    [Route("api/pacientes/{pacienteId}/historico")]
    [Authorize]
    public class HistoricoMedicoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HistoricoMedicoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Create(Guid pacienteId, [FromBody] CreateHistoricoDto dto)
        {
            var command = new CreateHistoricoCommand(pacienteId, dto.Diagnostico, dto.Exame, dto.Prescricao, dto.Data);
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetByPacienteId), new { pacienteId }, new { id });
        }

        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetByPacienteId(Guid pacienteId)
        {
            var historicos = await _mediator.Send(new GetHistoricosByPacienteIdQuery(pacienteId));
            return Ok(historicos);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Update(Guid pacienteId, Guid id, [FromBody] CreateHistoricoDto dto)
        {
            var command = new UpdateHistoricoCommand(id, dto.Diagnostico, dto.Exame, dto.Prescricao, dto.Data);
            var result = await _mediator.Send(command);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid pacienteId, Guid id)
        {
            var command = new DeleteHistoricoCommand(id);
            var result = await _mediator.Send(command);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
