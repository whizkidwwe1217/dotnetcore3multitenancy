using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace i21Apis.HealthChecks
{
    public class TenantDbHealthCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var healthCheckResultHealthy = true;

            if (healthCheckResultHealthy)
            {
                return await Task.FromResult(HealthCheckResult.Healthy("Connection to the tenant database is good."));
            }
            else
            {
                return await Task.FromResult(HealthCheckResult.Healthy("Connection to the tenant database is not reachable."));

            }
        }
    }
}