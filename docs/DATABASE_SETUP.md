# Local Database Setup — TECH-002

> Current status, reviewed 2026-11-02: TECH-002 is complete and published in
> 05e1abd. DbContext, credentials, health checks, migrations, persistence tests,
> and named-volume durability were verified. Walkthrough/troubleshooting text
> below is historical reference, not outstanding instructions. Do not recreate
> the database, reset credentials, or overwrite .env/compose.yaml from old snippets.
> Current Compose also includes the locally verified LGTM service under OBS-001.

Latest checkpoint: owner confirmed the intentional database-stop test:
liveness 200, readiness 503, readiness 200 after recovery. Follow
[migrations and persistence](PERSISTENCE_SETUP.md) next. Earlier troubleshooting
entries below are historical evidence, not current failures.

## Status and objective
Owner approved starting TECH-002 (#6) after the published TECH-001 checkpoint.
Owner created compose.yaml, .env.example, and the local .env. The misspelled
.env.exampl file is absent on disk. Direct inspection confirms Compose matches
the proposed configuration, and config --quiet completed without a configuration
error. Sandbox restrictions prevented Docker engine verification; the
outside-sandbox check was not approved. Owner subsequently provided successful
SQL output: database calliqo, user calliqo_dev, PostgreSQL 18.6 (Debian).
Compose health status was not included in that output; distinguish SQL
availability from the container health label. No database container or infrastructure file has been created
by Codex. EF Core/Npgsql, migrations, persistence, and API
readiness checks follow after this database-startup checkpoint.

Prerequisites: Docker Desktop's Linux engine running, repository root open
in PowerShell, and local host port 5432 available (or choose another port).
Complexity: low for this first checkpoint; overall task is moderate.

## Proposed local topology
React and the API continue running on the host. PostgreSQL alone runs in
Docker Compose. The host API will later connect to 127.0.0.1 and the chosen
host port. Future containers use the Compose service name postgres and its
container port 5432. Local Compose does not provision cloud infrastructure.

Use the official postgres:18.6 release tag. It selects a PostgreSQL patch
release but is not an immutable image digest. Review image updates deliberately;
cloud image pinning and infrastructure-as-code selection are later tasks.
PostgreSQL 18+ uses /var/lib/postgresql as its volume mount location.

## Owner file edits
### Repository root: .env.example
Format: dotenv. This tracked template contains placeholders, never real secrets.

```dotenv
POSTGRES_DB=calliqo
POSTGRES_USER=calliqo_dev
POSTGRES_PASSWORD=replace-with-local-dev-password
POSTGRES_PORT=5432
```

Copy it to the local ignored .env file, then replace the password in .env.
Use a unique development password; do not share it or use production secrets.
The .gitignore already ignores .env and permits .env.example.

```powershell
Copy-Item .env.example .env
```

The image-created POSTGRES_USER is a bootstrap superuser for this local
checkpoint. Before production, use a restricted application role and separate
migration/provisioning credentials. No production authorization is implied.

### Repository root: compose.yaml
Format: YAML. Depends on Docker Compose and the official PostgreSQL image.

```yaml
name: calliqo

services:
  postgres:
    image: postgres:18.6
    environment:
      POSTGRES_DB: ${POSTGRES_DB:-calliqo}
      POSTGRES_USER: ${POSTGRES_USER:-calliqo_dev}
      POSTGRES_PASSWORD: ${POSTGRES_PASSWORD:?Set POSTGRES_PASSWORD in .env}
    ports:
      - "127.0.0.1:${POSTGRES_PORT:-5432}:5432"
    volumes:
      - postgres_data:/var/lib/postgresql
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -h 127.0.0.1 -U $${POSTGRES_USER} -d $${POSTGRES_DB}"]
      interval: 5s
      timeout: 5s
      retries: 10
      start_period: 10s

volumes:
  postgres_data:
```

The named volume retains data when the container is recreated. It exists
only on this computer and is not copied through Git. Bind the published
port to loopback so other machines do not get a host-port listener.
The password is required. The double dollar signs defer healthcheck-variable
expansion to the container rather than substituting host values in Compose.
pg_isready measures database server readiness, not credentials, schema,
migrations, or application permissions. Those checks come with the API work.

## Validate and start — owner commands
Run from the repository root after replacing the local password:

```powershell
git check-ignore .env
docker compose config --quiet
docker compose up -d --wait postgres
docker compose ps
docker compose exec postgres psql -U calliqo_dev -d calliqo -c "SELECT current_database(), current_user, version();"
```

The first command should print .env. config --quiet validates without
printing the resolved password. up downloads the image if needed, starts
the database, and waits for health. Expected service state: running/healthy.
The SQL command should report database calliqo, user calliqo_dev, and
PostgreSQL 18.6. If user/database names were customized, adapt that command.
This local in-container query does not verify host API authentication.

Acceptance for this checkpoint: Compose validates, the database is healthy,
and the SQL query succeeds. No EF migration or persistence claim is made yet.
Do not commit .env or paste resolved Compose configuration into chat.

## Troubleshooting and data handling
- Port conflict: change only POSTGRES_PORT in .env (for example 5433), then
  rerun up and use that port in the future host API connection string.
- Docker engine failure: start Docker Desktop; report its startup error.
- Initialization failure: inspect docker compose logs postgres and share
  relevant error lines with credentials redacted.
- POSTGRES_USER/DB/PASSWORD initialize a fresh volume only. Editing .env
  later does not change an already initialized database's user/password.
- Stop with docker compose stop. Normal docker compose down retains the
  named volume; do not add --volumes/-v unless intentionally deleting data.
- Never repurpose a PostgreSQL major-version volume without a migration plan.

## Remaining TECH-002 work and Git
After startup verification: choose compatible EF Core/Npgsql versions,
configure host API secrets, add DbContext and a narrowly scoped development
persistence exercise, apply migrations, and verify read/write durability.
Add liveness/readiness checks and preserve test isolation. Agree the temporary
persistence exercise before creating football-domain entities scheduled later.
Coaches' team data must not become anonymously exposed through a demo endpoint.

Record actual results before marking acceptance checkboxes. Owner reviews
and commits compose.yaml, .env.example, and documentation referencing #6.
The local .env and database data stay outside Git. The current documentation
updates are local until the owner commits and pushes them.

## Official references
- https://hub.docker.com/_/postgres
- https://docs.docker.com/compose/how-tos/environment-variables/variable-interpolation/
- https://docs.docker.com/compose/how-tos/startup-order/

## EF Core/Npgsql tooling — next owner checkpoint
Status: owner installed the packages/tool. Direct inspection confirms EF Core
10.0.14, Design 10.0.14 (PrivateAssets=all), Npgsql provider 10.0.3, and a local
root dotnet-tools.json pinning dotnet-ef 10.0.14. Owner output confirms tool
version, build, existing test, and clean NuGet audits for API/test projects.
PostgreSQL SQL query passed. No API DbContext, migration, or persistence exists yet.

EF Core supplies the object/database mapping; Npgsql is the PostgreSQL
provider. Design supplies migration-generation support. dotnet-ef is the
CLI tool, pinned locally so another computer can use dotnet tool restore.
Provider 10.0.3 declares EF Core >=10.0.4 and <11.0.0; EF packages/tools
10.0.14 are within that major range. Exact project restore/build compatibility
and audit results remain to be verified by the owner.

From repository root:

```powershell
dotnet new tool-manifest
dotnet tool install --local dotnet-ef --version 10.0.14
dotnet add server/Calliqo.Api/Calliqo.Api.csproj package Microsoft.EntityFrameworkCore --version 10.0.14
dotnet add server/Calliqo.Api/Calliqo.Api.csproj package Microsoft.EntityFrameworkCore.Relational --version 10.0.14
dotnet add server/Calliqo.Api/Calliqo.Api.csproj package Npgsql.EntityFrameworkCore.PostgreSQL --version 10.0.3
dotnet add server/Calliqo.Api/Calliqo.Api.csproj package Microsoft.EntityFrameworkCore.Design --version 10.0.14
```

The installed SDK created dotnet-tools.json at the repository root (rather
than the previously anticipated .config location). Package references are saved to
server/Calliqo.Api/Calliqo.Api.csproj. If a manifest now exists, reuse it
rather than overwrite it. Ensure the Design package reference has
PrivateAssets=all (normally added by the package metadata) so tooling
does not flow to consumers as an application dependency.

Verify:

```powershell
dotnet ef --version
dotnet build Calliqo.slnx
dotnet test Calliqo.slnx
dotnet list Calliqo.slnx package --include-transitive --vulnerable
```

Expected: EF tool 10.0.14, successful build and existing integration test,
no unresolved audit findings. This step changes tooling/dependencies only,
not the database schema. Configure secrets and DbContext after this checkpoint;
do not put a real connection string in tracked configuration or chat.

Registry/official references:
- https://www.nuget.org/packages/Npgsql.EntityFrameworkCore.PostgreSQL/10.0.3
- https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design
- https://www.nuget.org/packages/dotnet-ef
- https://learn.microsoft.com/en-us/ef/core/cli/dotnet

## DbContext and local connection settings — owner walkthrough
Status: owner added the empty context, DI registration, and UserSecretsId.
Direct source inspection confirms those changes; no secret contents were read.
Owner output confirms build and the existing test pass, and EF reports
CalliqoDbContext/Npgsql with database calliqo at 127.0.0.1:5432.
This verifies context configuration, not a database connection attempt.
API does not automatically load
Compose's .env. Configure a separate ASP.NET Core Development user secret.
User Secrets are local, unencrypted development settings outside the repo;
use a cloud secret store later. Each computer sets its own local secret.

From repository root, initialize a stable project secret ID and placeholder:

```powershell
dotnet user-secrets init --project server/Calliqo.Api --id calliqo-api-development
dotnet user-secrets set "ConnectionStrings:Calliqo" "CONFIGURE_LOCALLY" --project server/Calliqo.Api
```

This adds UserSecretsId to the API csproj and creates a local secrets entry.
In VS Code, open this file outside the repo:
%APPDATA%\Microsoft\UserSecrets\calliqo-api-development\secrets.json
Replace its placeholder contents with the JSON below and privately replace
REPLACE_LOCALLY with the same local password used in .env. If the file already
contains other secrets, preserve them. Adapt Port if POSTGRES_PORT differs.

```json
{
  "ConnectionStrings": {
    "Calliqo": "Host=127.0.0.1;Port=5432;Database=calliqo;Username=calliqo_dev;Password=REPLACE_LOCALLY"
  }
}
```

Never paste the real value into chat or tracked configuration. Passwords
containing connection-string delimiters need proper connection-string quoting
and JSON escaping; request guidance using only the delimiter types if needed.

Create server/Calliqo.Api/Infrastructure/Persistence/CalliqoDbContext.cs
(C#, depends on the installed EF Core package):

```csharp
using Microsoft.EntityFrameworkCore;

namespace Calliqo.Api.Infrastructure.Persistence;

public sealed class CalliqoDbContext : DbContext
{
    public CalliqoDbContext(DbContextOptions<CalliqoDbContext> options)
        : base(options)
    {
    }
}
```

The context is intentionally empty; no football-domain table is introduced.
At the top of server/Calliqo.Api/Program.cs, add:

```csharp
using Calliqo.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
```

After builder.Services.AddOpenApi(), before builder.Build(), add:

```csharp
builder.Services.AddDbContext<CalliqoDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("Calliqo")
        ?? throw new InvalidOperationException(
            "Connection string 'Calliqo' is not configured.")));
```

The scoped context uses Npgsql; configuration is read when its options are
resolved, rather than forcing every unrelated endpoint/test to have a secret.
The weatherforecast integration test does not resolve the context and must
continue to run without a live database. Do not add startup migrations or
EnsureCreated to work around this; explicit migrations follow later.

Verify from the repository root:

```powershell
dotnet build Calliqo.slnx
dotnet test Calliqo.slnx
dotnet ef dbcontext info --project server/Calliqo.Api -- --environment Development
```

Expected: build/test pass; EF reports CalliqoDbContext and the Npgsql provider.
Do not print or share the actual connection string. dbcontext info verifies
context creation/configuration, not a successful database login or SQL query.
Actual API-to-database readiness, migrations, and read/write durability remain
pending. No application/configuration files have been changed by Codex.

References:
- https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-10.0
- https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/

## API liveness and database readiness — owner walkthrough
Status: owner implemented the health-check class and endpoint registrations;
direct inspection confirms the wiring. Screenshot verifies liveness 200 and
readiness 503. Owner confirms the database was not intentionally stopped;
the failure is unexpected and needs diagnosis. Successful
readiness remains unverified even after the owner starts Compose: screenshot
shows PostgreSQL healthy at 127.0.0.1:5432 and API readiness still 503.
pg_isready health is not proof of successful API username/password authentication.
Next read-only diagnostic from the repository root:

```powershell
dotnet ef migrations list --project server/Calliqo.Api -- --environment Development
```

This lists migrations and attempts database access without applying migrations
or changing schema. With no migrations yet, No migrations were found is
expected; include any preceding connection warning/error when reporting.
Do not pass or print the connection string on the command line.

Owner diagnostic subsequently confirmed SQLSTATE 28P01: password authentication
failed for calliqo_dev. Fix the credential, not the schema. If the original
database password is known, privately correct the local User Secret and
restart the API. Changing POSTGRES_PASSWORD in .env alone does not change
the password stored in an already initialized volume.

To reset the existing local bootstrap role without deleting data, the owner
can open psql over its container-local Unix socket from the repository root:

```powershell
docker compose exec postgres psql -U calliqo_dev -d calliqo
```

At the psql prompt (not the PowerShell prompt):

```text
\password calliqo_dev
```

Enter the new local password twice in the hidden prompts, then exit with
\q. This changes the role password and preserves database contents.
Update POSTGRES_PASSWORD in local .env for consistency and the Password
field in the local User Secret to that same value. Preserve the connection
string's host/port/database/user. Correctly escape delimiters in the
connection string and JSON; never paste credentials into chat or Git.
Restart the API, rerun EF migrations list, and request /health/ready.
Expected: no authentication warning, no migrations yet, readiness 200.
No password reset or settings change has been performed by Codex.

After owner credential changes, the next EF migrations-list output no longer
showed 28P01. It logged a failed SELECT from __EFMigrationsHistory and reported
no migrations. This is consistent with the history table not existing before
the first migration; the actual SQLSTATE was not included, so the cause is
not conclusively verified. Do not manually create that table. Restart the
API and verify /health/ready; migrations will create/manage their history.

Latest result: owner confirms /health/ready is Healthy after credential
correction. Successful host API connectivity is now verified. The earlier
503 was an authentication problem, not an intentional database-stop test.
Still verify liveness 200/readiness 503 with PostgreSQL deliberately stopped,
then readiness 200 after restarting. Migrations and persistence remain pending.

Successful
readiness and recovery remain pending. Objective:
verify host API database connectivity separately from API process liveness.
Uses ASP.NET Core's built-in health-check services and existing EF packages.

Create server/Calliqo.Api/Infrastructure/Observability/DatabaseReadinessHealthCheck.cs
(C#, depends on CalliqoDbContext, EF Core, and built-in IHealthCheck):

```csharp
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
```

Add these imports at the top of server/Calliqo.Api/Program.cs:

```csharp
using Calliqo.Api.Infrastructure.Observability;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
```

After AddDbContext registration and before builder.Build(), add:

```csharp
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseReadinessHealthCheck>(
        "database",
        tags: ["ready"],
        timeout: TimeSpan.FromSeconds(5));
```

Before app.Run(), add:

```csharp
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});
```

The liveness endpoint runs no dependency checks. Readiness resolves the
scoped DbContext and tests its configured connection. Defaults return
Healthy/200 or Unhealthy/503 without exposing connection details.
CanConnectAsync does not verify schema/migrations or read/write permissions.
The five-second timeout is passed through the cancellation token.

Verify from repository root (stop any previous API process before rebuilding):

```powershell
dotnet build Calliqo.slnx
dotnet test Calliqo.slnx
dotnet run --project server/Calliqo.Api --launch-profile http
```

Keep the API running. In a second PowerShell terminal at the repository root:

```powershell
curl.exe -i http://localhost:5033/health/live
curl.exe -i http://localhost:5033/health/ready
docker compose stop postgres
curl.exe -i http://localhost:5033/health/live
curl.exe -i http://localhost:5033/health/ready
docker compose up -d --wait postgres
curl.exe -i http://localhost:5033/health/ready
```

Expected sequence: 200/Healthy for both, then liveness 200 and readiness
503/Unhealthy while PostgreSQL is stopped, then readiness 200 after recovery.
The stop command is owner-run and preserves the volume. Always bring
PostgreSQL back up after the check. API health checks run only when requested.
Use the actual launch port if settings differ. No certificate is involved
when using the HTTP profile. A missing HTTPS redirect-port warning is separate.
Share statuses/errors, not secrets. No endpoint or health implementation has
been applied by Codex. Persistence/migrations remain after this checkpoint.

References:
- https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-10.0
- https://learn.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.infrastructure.databasefacade.canconnectasync?view=efcore-10.0
