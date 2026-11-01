# PostgreSQL Persistence Test — TECH-002

## Status and prerequisites
Owner implemented and ran the test; no test package/source changes by Codex.
Owner output confirms two passing tests, warning-free build, and clean
NuGet audits for API and tests. Testcontainers starts PostgreSQL and removes
the disposable test container afterward. EF migration/write/fresh-context
read are verified; Compose-volume recreation durability remains pending.
The earlier EF Relational mismatch is resolved: both projects use 10.0.14,
warning-free build and one existing test pass, and NuGet audits are clean.
InitialPersistence is applied locally and its source is available.
Docker Desktop's Linux engine must be running.

## Purpose
Test the actual migration and an EF write/read against an isolated database.
Use a fresh context for the read to avoid satisfying assertions from tracked
objects. Never point this test at the owner's development database.
Testcontainers uses a disposable PostgreSQL instance and dynamic host port,
without .env or User Secrets. No public API endpoint is introduced.

## Install — owner command from repository root
Modifies tests/Calliqo.IntegrationTests/Calliqo.IntegrationTests.csproj:

```powershell
dotnet add tests/Calliqo.IntegrationTests/Calliqo.IntegrationTests.csproj package Testcontainers.PostgreSql --version 4.16.0
```

## Create tests/Calliqo.IntegrationTests/PersistenceTests.cs
Language: C#. Dependencies: xUnit v2 IAsyncLifetime; EF/Npgsql and the context,
entity, and migration through the API project reference; Testcontainers.PostgreSql.
The test-only password below is for its disposable container, not a real secret.

```csharp
using Calliqo.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using Xunit;

namespace Calliqo.IntegrationTests;

public sealed class PersistenceTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres =
        new PostgreSqlBuilder("postgres:18.6")
            .WithDatabase("calliqo_tests")
            .WithUsername("calliqo_test")
            .WithPassword("test-only-password")
            .Build();

    public Task InitializeAsync() => _postgres.StartAsync();

    public Task DisposeAsync() => _postgres.DisposeAsync().AsTask();

    [Fact]
    public async Task SavesAndReadsProbeUsingSeparateContexts()
    {
        var id = Guid.NewGuid();
        var createdAt = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);

        await using (var writer = CreateContext())
        {
            await writer.Database.MigrateAsync();
            writer.PersistenceProbes.Add(new PersistenceProbe
            {
                Id = id,
                CreatedAtUtc = createdAt
            });

            Assert.Equal(1, await writer.SaveChangesAsync());
        }

        await using var reader = CreateContext();
        var saved = await reader.PersistenceProbes
            .AsNoTracking()
            .SingleAsync(probe => probe.Id == id);

        Assert.Equal(id, saved.Id);
        Assert.Equal(createdAt, saved.CreatedAtUtc);
    }

    private CalliqoDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<CalliqoDbContext>()
            .UseNpgsql(_postgres.GetConnectionString())
            .Options;

        return new CalliqoDbContext(options);
    }
}
```

Initialization starts the isolated database; teardown removes it. MigrateAsync
applies the real migration to that test database only. The writer is disposed
before a fresh no-tracking context reads from PostgreSQL. The fixed UTC
timestamp avoids precision-rounding differences during the round trip.
This does not verify the Compose named volume across container recreation.

## Verify — owner commands from repository root

```powershell
dotnet test Calliqo.slnx
dotnet build Calliqo.slnx
dotnet list Calliqo.slnx package --include-transitive --vulnerable
```

Expected: two passing tests, warning-free build, no unreviewed audit findings.
The first run may download PostgreSQL and Testcontainers cleanup-helper images.
Do not disable cleanup as a workaround. Report startup, dependency, or test
errors before proceeding. No result is claimed until execution is verified.
After this, check Compose-volume durability separately, then review the
owner-created TECH-002 checkpoint commit. Keep .env and User Secrets out of Git.

References:
- https://dotnet.testcontainers.org/modules/postgres/
- https://www.nuget.org/packages/Testcontainers.PostgreSql/4.16.0
