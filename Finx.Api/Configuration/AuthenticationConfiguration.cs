using System.Text;
using Finx.Api.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Finx.Api.Configuration
{
    public static class AuthenticationConfiguration
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSecret = configuration["Jwt:Secret"];

            // Development-safe fallback: HS256 requires >= 32 bytes.
            // In production, configure Jwt:Secret via appsettings/secret store/env var.
            if (string.IsNullOrWhiteSpace(jwtSecret))
                jwtSecret = "very_secret_key_for_dev_only_32_bytes_min!";

            jwtSecret = jwtSecret.Trim();
            var keyBytes = TryReadBase64(jwtSecret) ?? Encoding.UTF8.GetBytes(jwtSecret);

            services.AddSingleton(new JwtTokenService(jwtSecret));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
                };
            });

            services.AddAuthorization();

            return services;
        }

        private static byte[]? TryReadBase64(string value)
        {
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
