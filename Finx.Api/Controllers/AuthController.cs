using Finx.Api.DTOs;
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
        public IActionResult Login([FromBody] LoginRequest req)
        {
            // Simple hardcoded validation for demo — replace with real user store
            if (req.Username == "admin" && req.Password == "admin")
            {
                var token = _tokens.GenerateToken(req.Username, "Admin");
                return Created(string.Empty, new { token, expiresInHours = 2 });
            }

            if (req.Username == "user" && req.Password == "user")
            {
                var token = _tokens.GenerateToken(req.Username, "User");
                return Created(string.Empty, new { token, expiresInHours = 2 });
            }

            return Unauthorized();
        }
    }
}
