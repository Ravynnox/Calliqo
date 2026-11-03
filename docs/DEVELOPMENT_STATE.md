# Development State

## NEXT SESSION START HERE
Read AGENTS.md, this file, ROADMAP.md, ARCHITECTURE.md, DECISIONS.md,
WORK_TRACKING.md, and the current issue. Inspect Git before reporting state.
Maintain documentation automatically; the owner controls code/configuration
and Git mutations. Never read or display local credentials unnecessarily.

## Current work
- Phase: 1 — Technical Foundation
- Milestone: API conventions and CI
- Task: TECH-003 — API conventions and CI, issue #7
- Status: In progress — root workflow placement/content verified; publication and CI pending
- Last completed task: TECH-002 — API/database connection, issue #6
- Last update: 2026-11-02

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

## Files in the published TECH-002 checkpoint
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
- Last verified published checkpoint: 05e1abddcadf360209a0dbbd508d6aff1ca2c1a6.
- TECH-002 source/configuration/tests and associated documentation are published.
- Working tree was clean before the current completion/walkthrough doc updates.
- Current documentation updates are local; include them in the next checkpoint.
- Previous TECH-001 completion-note updates are included in this checkpoint.
- Issue #6 completion is verified; close it and move its card to Done.
- TECH-003 (#7) is current; Problem Details, versioned routing, and OpenAPI
  contract tests are implemented locally by the owner.
- Test class WeatherForecastTests remains in UnitTest1.cs; filename cleanup is optional.

## Work remaining
- Owner reports all tests passed after correcting both forecast caller URLs.
  Versioned route/OpenAPI checkpoint is passing per owner report. No new
  test execution was performed by Codex.
- Weather-validation walkthrough is superseded by owner agreement. No days-query
  implementation or 16-test target remains. Real request validation is tracked
  under AUTH-002 (#10), including field errors, traceId, and contract tests.
- Owner corrected workflow location to .github/workflows/ci.yml, confirmed by
  Test-Path and file enumeration. It is the only ci.yml found. Content matches
  the prepared frontend/backend workflow. Source diffs and new tests reviewed.
  Git diff --check passes; line-ending notices remain informational.
- Workflow is untracked; owner publication and remote CI verification remain pending.
- Keep health endpoints operational and diagnostics free of secrets.
- Complete second-computer checks when available under FND-003.
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
Owner reviews/stages the intended API, tests, root workflow, and documentation,
checks the staged diff, then commits/pushes a TECH-003 checkpoint referencing #7.
After publication inspect CI jobs for the intended commit. Keep #7 open pending
both successful jobs and documentation publication. No code/configuration/Git
mutations or CI executions were performed by Codex.

## Completion rule
A task is not finalized until associated implementation and documentation
are committed. Cross-computer continuity additionally requires push/retrieval
verification. Planned or untested work must never be presented as complete.

## Tracking reconciliation — 2026-11-02
Inspected all 24 GitHub issues: no standalone weather-demo issue exists.
Updated issue #7 to cancel that exercise and retain CI/publication work;
updated #10 with real-endpoint validation acceptance criteria. Read-back
verified both remote issue bodies match their local snapshots; both remain
open. No issue closure was warranted. No code or Git mutations performed.
