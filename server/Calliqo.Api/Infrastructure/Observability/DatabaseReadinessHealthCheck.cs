using Calliqo.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Calliqo.Api.Infrastructure.Observability;

public sealed class DatabaseReadinessHealthCheck : IHealthCheck
{
    private readonly CalliqoDbContext _database;

    public DatabaseReadinessHealthCheck(CalliqoDbContext database)
    {
        _database = database;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var connected = await _database.Database.CanConnectAsync(cancellationToken);
        return connected
            ? HealthCheckResult.Healthy()
            : HealthCheckResult.Unhealthy("Database unavailable.");
    }
}