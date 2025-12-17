using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Finx.Integrations.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace Finx.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamesController : ControllerBase
    {
        private readonly IExameClient _exameClient;

        public ExamesController(IExameClient exameClient)
        {
            _exameClient = exameClient;
        }

        [HttpGet("{cpf}")]
        public async Task<IActionResult> GetByCpf(string cpf)
        {
            var exames = await _exameClient.GetExamesByCpfAsync(cpf);
            return Ok(exames);
        }
    }
}
