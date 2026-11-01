# Migrations and Persistence — TECH-002

## Latest checkpoint
Owner confirms the exact marker row survives Compose down/up without volume
deletion. Alongside applied/listed migration, passing EF persistence test,
warning-free build, and clean audits, implementation checks are complete.
Owner review, commit/push, and publication verification remain. Earlier
checkpoint notes below preserve the implementation/troubleshooting sequence.

## Status
Owner confirmed liveness 200 and readiness 503 during an intentional
PostgreSQL stop, then readiness 200 after restart. Connectivity and health
separation are verified. This migration walkthrough is proposed only;
Owner subsequently created the model/migration and supplied successful
database update output: 20261101203949_InitialPersistence applied, Done.
An initial failed history-table SELECT preceded successful application.
Owner subsequently supplied migration listing showing it applied and a passing
existing test. Build/test emitted MSB3277: API resolves EF Relational 10.0.14,
test project resolves 10.0.4. Direct assets inspection confirms the mismatch.
Owner added the explicit API Relational 10.0.14 reference. Latest supplied
output confirms warning-free build, one passing existing test, both projects
resolving Relational 10.0.14, and clean NuGet audits. EF record write/read
and volume durability remain pending. Follow PERSISTENCE_TESTS.md next.

Latest checkpoint: owner implemented the isolated persistence test. Two tests
pass, build is warning-free, and both NuGet audits are clean. EF migration,
write, and fresh-context read are verified. Only Compose-volume durability
and the owner-reviewed task checkpoint remain before final acceptance review.

## Purpose and scope
Use a small infrastructure-only PersistenceProbe table to exercise the
database plumbing without prematurely defining teams, playbooks, or players.
It stores only an identifier and UTC timestamp, has no API endpoint, and is
not a customer feature. After migrations, verify an EF save and read with
a new context, then durability across local database restart. Use a separate
test database/container for automated tests; never reset the development
database from an automated test.

The probe is temporary. Review its removal through a later migration when
a real module has equivalent persistence tests; remove it before MVP release.
Do not manually drop the table or rewrite already-published migrations.

## Prerequisites
TECH-002 context/tool setup complete; PostgreSQL healthy; Development user
secret configured privately. Stop the API with Ctrl+C while generating/building
the migration, leaving PostgreSQL running. Owner applies all code and database changes.

## 1. Create the technical entity
Destination: server/Calliqo.Api/Infrastructure/Persistence/PersistenceProbe.cs
Language: C#. Uses framework Guid and DateTimeOffset types.

```csharp
namespace Calliqo.Api.Infrastructure.Persistence;

public sealed class PersistenceProbe
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
}
```

## 2. Replace the empty context
Destination: server/Calliqo.Api/Infrastructure/Persistence/CalliqoDbContext.cs
Language: C#. Depends on EF Core and PersistenceProbe. Retains the existing
options constructor used by Npgsql dependency injection.

```csharp
using Microsoft.EntityFrameworkCore;

namespace Calliqo.Api.Infrastructure.Persistence;

public sealed class CalliqoDbContext : DbContext
{
    public CalliqoDbContext(DbContextOptions<CalliqoDbContext> options)
        : base(options)
    {
    }

    public DbSet<PersistenceProbe> PersistenceProbes => Set<PersistenceProbe>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var probe = modelBuilder.Entity<PersistenceProbe>();
        probe.ToTable("persistence_probes", "infrastructure");
        probe.HasKey(x => x.Id);
        probe.Property(x => x.Id).HasColumnName("id");
        probe.Property(x => x.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .HasColumnType("timestamp with time zone");
    }
}
```

DbSet exposes the entity for EF operations. Mapping defines its schema,
table, primary key, and column names. Use DateTimeOffset.UtcNow when writing
the probe so the PostgreSQL timestamp value has a zero UTC offset.

## 3. Generate and inspect the migration
Run from repository root. First command generates source files; the second
prints SQL for review without applying it:

```powershell
dotnet ef migrations add InitialPersistence --project server/Calliqo.Api --output-dir Infrastructure/Persistence/Migrations -- --environment Development
dotnet ef migrations script 0 InitialPersistence --project server/Calliqo.Api -- --environment Development
```

Inspect the generated migration and snapshot under Infrastructure/Persistence/Migrations.
The Up method should ensure the infrastructure schema and create one table,
persistence_probes, with UUID id primary key and timestamp-with-time-zone
created_at_utc. SQL also creates/uses __EFMigrationsHistory. The Down method
is the reverse operation; its existence does not mean rollback has been run.
Stop if unexpected application tables or destructive Up operations appear.

## 4. Apply to the local database and verify
The update command changes the local schema using the configured Development
connection. It is not a production deployment strategy. Once the generated
changes match the description, the owner runs:

```powershell
dotnet ef database update --project server/Calliqo.Api -- --environment Development
dotnet ef migrations list --project server/Calliqo.Api -- --environment Development
dotnet build Calliqo.slnx
dotnet test Calliqo.slnx
```

Expected: InitialPersistence applies and is listed without Pending; build
and the existing forecast test pass. EF manages its own migration history.
Do not use EnsureCreated or manually create __EFMigrationsHistory.
Report results and errors without credentials. No EF write/read or volume
durability has been verified by merely applying the migration.

## Remaining verification and Git
### Resolve EF runtime package alignment first
The earlier package instructions omitted an explicit Relational runtime
reference. Design brings version 10.0.14 into the API, but PrivateAssets=all
prevents that tooling dependency from flowing to tests. Npgsql's minimum
Relational dependency then resolves 10.0.4 in the consuming test project.
Retain Design PrivateAssets=all and fix the runtime declaration at its source.

Owner commands from repository root (first command modifies the API csproj):

```powershell
dotnet add server/Calliqo.Api/Calliqo.Api.csproj package Microsoft.EntityFrameworkCore.Relational --version 10.0.14
dotnet build Calliqo.slnx
dotnet test Calliqo.slnx
dotnet list Calliqo.slnx package --include-transitive
dotnet list Calliqo.slnx package --include-transitive --vulnerable
```

Expected: both projects resolve Relational 10.0.14; build/test pass without
MSB3277 and audits remain clean. No direct test-project pin is needed.
No package reference has been changed by Codex. No migration regeneration or
database changes are needed for this package-alignment fix.
Reference: https://learn.microsoft.com/en-us/ef/core/what-is-new/nuget-packages

Next checkpoint: EF SaveChangesAsync followed by a read from a fresh context,
isolated PostgreSQL integration testing, and local volume durability. No
public diagnostic route is needed. Final acceptance includes reproducible
setup documentation, audit/check results, and an owner-reviewed commit/push
referencing TECH-002 (#6). Keep .env and User Secrets outside Git.

References:
- https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/
- https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying

## Local named-volume durability — owner verification
Prerequisites: local InitialPersistence applied, PostgreSQL running, test
table contains technical probe records only. Run from repository root.
This is a manual infrastructure check distinct from the EF integration test.

Insert a technical marker and record its returned identifier/timestamp:

```powershell
docker compose exec -T postgres psql -U calliqo_dev -d calliqo -c "INSERT INTO infrastructure.persistence_probes (id, created_at_utc) VALUES (gen_random_uuid(), now()) RETURNING id, created_at_utc;"
```

Recreate the Compose container/network while retaining the named data volume:

```powershell
docker compose down
docker compose up -d --wait postgres
docker compose exec -T postgres psql -U calliqo_dev -d calliqo -c "SELECT id, created_at_utc FROM infrastructure.persistence_probes ORDER BY created_at_utc;"
```

Use plain down only: do not add -v/--volumes, which would remove data volumes.
This briefly stops the local database and therefore makes API readiness fail
if the API is running. After restart, the exact inserted ID and timestamp
must remain. This verifies volume persistence across container recreation,
not backup/disaster recovery. No commands have been executed by Codex.
Report the row comparison and container status before completing TECH-002.
