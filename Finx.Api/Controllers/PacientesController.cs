using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Finx.Api.DTOs;
using Finx.Api.Handlers;

namespace Finx.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacientesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PacientesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePacienteDto dto)
        {
            var command = new CreatePacienteCommand(dto.Nome, dto.Cpf, dto.DataNascimento, dto.Contato);
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            // placeholder, implement query handler later
            return Ok(new { id });
        }
    }
}