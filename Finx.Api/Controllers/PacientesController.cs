using MediatR;
using Finx.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Finx.Api.Handlers.Pacientes.Queries;
using Finx.Api.Handlers.Pacientes.Commands;

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

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePacienteDto dto)
        {
            var command = new UpdatePacienteCommand(id, dto.Nome, dto.Cpf, dto.DataNascimento, dto.Contato);
            var result = await _mediator.Send(command);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeletePacienteCommand(id);
            var result = await _mediator.Send(command);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
