using Finx.Api.Model;
using Finx.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Finx.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenService _tokens;

        public AuthController(JwtTokenService tokens)
        {
            _tokens = tokens;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            // Simple hardcoded validation for demo — replace with real user store
            if (loginRequest.Username == "admin" && loginRequest.Password == "admin")
            {
                var token = _tokens.GenerateToken(loginRequest.Username, "Admin");
                return Created(string.Empty, new { token, expiresInHours = 2 });
            }

            if (loginRequest.Username == "user" && loginRequest.Password == "user")
            {
                var token = _tokens.GenerateToken(loginRequest.Username, "User");
                return Created(string.Empty, new { token, expiresInHours = 2 });
            }

            return Unauthorized();
        }
    }
}
