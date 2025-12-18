using Microsoft.AspNetCore.Mvc;
using Finx.Integrations.Contracts;
using Microsoft.AspNetCore.Authorization;
using Finx.Api.Configuration;

namespace Finx.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExamesController : ControllerBase
    {
        private readonly IExameClient _exameClient;

        public ExamesController(IExameClient exameClient)
        {
            _exameClient = exameClient;
        }

        [HttpGet("{cpf}")]
        [Authorize(Policy = AuthorizationPolicies.AdminOrUser)]
        public async Task<IActionResult> GetByCpf(string cpf)
        {
            var exames = await _exameClient.GetExamesByCpfAsync(cpf);
            if (exames == null) return NoContent();
            return Ok(exames);
        }
    }
}
