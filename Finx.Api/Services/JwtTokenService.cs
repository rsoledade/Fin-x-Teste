using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Finx.Api.Services
{
    public class JwtTokenService
    {
        private readonly byte[] _keyBytes;

        public JwtTokenService(string secret)
        {
            if (string.IsNullOrWhiteSpace(secret))
                throw new ArgumentException("JWT secret must be provided.", nameof(secret));

            secret = secret.Trim();

            _keyBytes = TryReadBase64(secret) ?? Encoding.UTF8.GetBytes(secret);

            if (_keyBytes.Length < 32)
            {
                throw new ArgumentException(
                    $"JWT secret is too short for HS256. Provide at least 32 bytes (256 bits). Current: {_keyBytes.Length} bytes.",
                    nameof(secret));
            }
        }

        public string GenerateToken(string subject, string role)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, subject),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(_keyBytes);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static byte[]? TryReadBase64(string value)
        {
            // If the secret is provided as base64, allow it; otherwise fall back to raw UTF-8.
            try
            {
                return Convert.FromBase64String(value);
            }
            catch (FormatException)
            {
                return null;
            }
        }
    }
}
