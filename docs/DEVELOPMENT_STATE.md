# Development State

## NEXT SESSION START HERE
Read AGENTS.md, this file, ROADMAP.md, ARCHITECTURE.md, DECISIONS.md,
WORK_TRACKING.md, and the current issue. Inspect Git before reporting state.
Maintain documentation automatically; the owner controls code/configuration
and Git mutations. Never read or display local credentials unnecessarily.

## Current work
- Phase: 1 — Technical Foundation
- Milestone: API/database connection
- Task: TECH-002 — API/database connection, issue #6
- Status: Implementation verified; awaiting owner checkpoint commit/push
- Last completed task: TECH-001 — Application scaffolds, issue #5
- Last update: 2026-11-01

## Work completed
- TECH-001 scaffold/version/baseline-test checkpoint is published as c06c03d.
- GitHub work tracking is established; FND-004 (#4) is complete.
- Owner implemented PostgreSQL 18.6 in Compose with loopback port 5432,
  required local password, health check, and named volume.
- Owner configured Development User Secrets outside Git, an empty initial
  DbContext, and scoped Npgsql registration.
- EF Core/Design/Relational 10.0.14 and Npgsql provider 10.0.3 are installed.
  Root dotnet-tools.json pins dotnet-ef 10.0.14.
- Owner implemented /health/live and /health/ready. Manual checks confirm
  live 200 and ready 503 while PostgreSQL is stopped, ready 200 after recovery.
- Authentication error 28P01 was resolved by owner credential correction;
  no secret contents were read or changed by Codex.
- InitialPersistence migration 20261101203949 is applied and listed.
  It creates infrastructure.persistence_probes with UUID ID and UTC timestamp.
- Owner added isolated Testcontainers PostgreSQL persistence test:
  real migration, EF save, writer disposal, and fresh-context/no-tracking read.
- Owner confirms the local marker row survived Compose container removal
  and recreation while retaining the named data volume.
- Documentation and task evidence are updated. No application/configuration,
  database, or Git mutations were performed by Codex.

## Verification
- Frontend: one Vitest interaction test, build, lint, and npm audit passed
  per earlier owner results. Temporary minimatch override rationale: ADR-001.
- Backend: two tests passed, zero failed/skipped; warning-free solution build.
- Both API/test projects resolve EF Relational 10.0.14; MSB3277 is resolved.
- NuGet audit: no vulnerable packages in either project from current sources.
- Database startup, SQL response, migration application, health separation,
  isolated EF persistence, and Compose-volume durability verified as above.
- .env is ignored and untracked. .env.example uses the expected placeholder.
- Secrets remain local; API does not automatically read Compose's .env.
- Current documentation diff passes whitespace checks.
- Second-computer verification is deferred under FND-003 (#3).
- Durability checks are not backup/restore or production readiness verification.

## Files in the TECH-002 checkpoint
- compose.yaml; .env.example; dotnet-tools.json.
- server/Calliqo.Api/Calliqo.Api.csproj and Program.cs.
- server/Calliqo.Api/Infrastructure/Persistence/: context, probe, migration,
  migration designer, and model snapshot.
- server/Calliqo.Api/Infrastructure/Observability/DatabaseReadinessHealthCheck.cs.
- tests/Calliqo.IntegrationTests/: project reference changes and PersistenceTests.cs.
- README.md and docs/: current state, architecture, roadmap/tracking, testing,
  DATABASE_SETUP.md, PERSISTENCE_SETUP.md, PERSISTENCE_TESTS.md, and issue snapshots.
- .env and local User Secrets must not be staged.

## Repository status
- Branch: main; origin: https://github.com/Ravynnox/Calliqo.git.
- Last verified published checkpoint: c06c03d7076f9982d5cfaa4917dd1e925041d360.
- TECH-002 source/configuration/tests and current documentation are uncommitted.
- Previous TECH-001 completion-note updates are included in this checkpoint.
- Issue #6 remains open/In Progress pending owner review, commit, and push.
- Test class WeatherForecastTests remains in UnitTest1.cs; filename cleanup is optional.

## Work remaining
- Owner reviews/stages the intended files, verifies no credentials are staged,
  commits with a reference to #6, and pushes.
- Verify the actual published commit before completing issue #6.
- Obtain approval for TECH-003 — API conventions and CI (#7).
- Complete second-computer checks when available; no seamless handoff claim yet.

## Known follow-ups and outstanding decisions
- PersistenceProbe is a technical verification table, not a football feature.
  Remove it through a migration before MVP release once real module tests
  provide equivalent persistence coverage.
- Local bootstrap role is privileged. Production requires restricted runtime
  credentials and separate migration/provisioning permissions.
- Identity approach, renderer, cloud provider/IaC tool, telemetry storage/hosting,
  initial football rules, and browser/device matrix remain open decisions.
- Grafana, OpenTelemetry, cloud IaC, backups/restore, and CI remain planned.

## Next recommended action
Review and commit the TECH-002 checkpoint using the documented Git steps.
After verifying publication, close #6 and move its Project card to Done.
Then present TECH-003 and await owner approval before starting it.
Do not rerun passed checks without changes or a new unresolved concern.
Do not create a separate status-only commit after every observation; include
completion notes with the next meaningful owner-reviewed checkpoint.

## Completion rule
A task is not finalized until associated implementation and documentation
are committed. Cross-computer continuity additionally requires push/retrieval
verification. Planned or untested work must never be presented as complete.
