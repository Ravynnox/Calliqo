using Calliqo.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;

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
        try
        {
            await _database.Database.ExecuteSqlRawAsync(
                "SELECT 1",
                cancellationToken);

            return HealthCheckResult.Healthy();
        }
        catch (Exception exception)
            when (exception is NpgsqlException or TimeoutException)
        {
            return HealthCheckResult.Unhealthy("Database unavailable.");
        }
    }
}