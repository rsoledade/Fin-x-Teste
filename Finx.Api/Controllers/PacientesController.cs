using MediatR;
using Finx.Api.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Finx.Application.Handlers.Pacientes.Queries;
using Finx.Application.Handlers.Pacientes.Commands;
using Finx.Api.Configuration;

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
        [Authorize(Policy = AuthorizationPolicies.AdminOrUser)]
        public async Task<IActionResult> Create([FromBody] CreatePacienteRequest req)
        {
            var command = new CreatePacienteCommand(req.Nome, req.Cpf, req.DataNascimento, req.Contato);
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpGet]
        [Authorize(Policy = AuthorizationPolicies.AdminOrUser)]
        public async Task<IActionResult> GetAll()
        {
            var pacientes = await _mediator.Send(new GetPacientesQuery());
            return Ok(pacientes);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = AuthorizationPolicies.AdminOrUser)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var paciente = await _mediator.Send(new GetPacienteByIdQuery(id));
            if (paciente == null) return NotFound();
            return Ok(paciente);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = AuthorizationPolicies.AdminOrUser)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePacienteRequest req)
        {
            var command = new UpdatePacienteCommand(id, req.Nome, req.DataNascimento, req.Contato);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = AuthorizationPolicies.AdminOnly)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeletePacienteCommand(id));
            return NoContent();
        }
    }
}
