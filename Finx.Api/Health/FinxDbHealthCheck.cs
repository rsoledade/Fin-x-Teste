using Microsoft.Extensions.Diagnostics.HealthChecks;
using Finx.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Finx.Api.Health
{
    public class FinxDbHealthCheck : IHealthCheck
    {
        private readonly FinxDbContext _db;
        public FinxDbHealthCheck(FinxDbContext db) => _db = db;

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                if (await _db.Database.CanConnectAsync(cancellationToken)) return HealthCheckResult.Healthy();
                return HealthCheckResult.Unhealthy("Cannot connect to database");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy(ex.Message);
            }
        }
    }
}
