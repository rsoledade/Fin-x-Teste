using MediatR;
using Finx.Api.Model;
using Finx.Api.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Finx.Application.Handlers.Historico.Queries;
using Finx.Application.Handlers.Historico.Commands;

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
        [Authorize(Policy = AuthorizationPolicies.AdminOrUser)]
        public async Task<IActionResult> Create(Guid pacienteId, [FromBody] CreateHistoricoRequest createHistoricoRequest)
        {
            var command = new CreateHistoricoCommand(pacienteId, createHistoricoRequest.Diagnostico, createHistoricoRequest.Exame, createHistoricoRequest.Prescricao, createHistoricoRequest.Data);
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetByPacienteId), new { pacienteId }, new { id });
        }

        [HttpGet]
        [Authorize(Policy = AuthorizationPolicies.AdminOrUser)]
        public async Task<IActionResult> GetByPacienteId(Guid pacienteId)
        {
            var historicos = await _mediator.Send(new GetHistoricosByPacienteIdQuery(pacienteId));
            return Ok(historicos);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = AuthorizationPolicies.AdminOrUser)]
        public async Task<IActionResult> Update(Guid pacienteId, Guid id, [FromBody] CreateHistoricoRequest createHistoricoRequest)
        {
            var command = new UpdateHistoricoCommand(id, pacienteId, createHistoricoRequest.Diagnostico, createHistoricoRequest.Exame, createHistoricoRequest.Prescricao, createHistoricoRequest.Data);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = AuthorizationPolicies.AdminOnly)]
        public async Task<IActionResult> Delete(Guid pacienteId, Guid id)
        {
            await _mediator.Send(new DeleteHistoricoCommand(pacienteId, id));
            return NoContent();
        }
    }
}
