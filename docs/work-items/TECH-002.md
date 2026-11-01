# TECH-002 — API/database connection

GitHub issue: [#6](https://github.com/Ravynnox/Calliqo/issues/6)

## Objective
Connect the API to PostgreSQL with reproducible local container setup.

## Roadmap status
In progress. Owner approved guided setup; see docs/DATABASE_SETUP.md.

## Dependencies
[TECH-001 (#5)](https://github.com/Ravynnox/Calliqo/issues/5)

## Acceptance criteria
- [x] Docker Compose starts required local services using documented configuration.
- [x] EF Core migration and persistence/retrieval are verified against PostgreSQL.
- [x] Health checks distinguish application availability and database readiness.
- [x] Setup and troubleshooting instructions reproduce the workflow.

## Evidence and remaining work
TECH-001 dependency is complete and published in c06c03d. Owner approved
TECH-002. First checkpoint is local PostgreSQL 18.6 with Compose, an ignored
.env, a tracked placeholder .env.example, loopback binding, named storage,
and a readiness probe. Owner created Compose/environment files and supplied
successful SQL output showing calliqo/calliqo_dev on PostgreSQL 18.6.
Compose health and local SQL response are verified. Owner installed EF Core/Design
10.0.14, Npgsql provider 10.0.3, and root local dotnet-ef manifest (10.0.14).
Tool version, solution build, existing integration test, and NuGet audits pass.
Owner added DbContext/Npgsql registration and UserSecretsId; no secrets were
read by Codex. Build/test/context info pass, reporting calliqo at 127.0.0.1:5432.
Owner implemented API liveness/readiness. Liveness 200 was observed; initial
readiness 503 was traced to SQLSTATE 28P01 and resolved by owner credential
correction. Owner confirmed intentional outage/recovery: live 200, ready 503,
then ready 200 after restart. InitialPersistence is applied/listed. API/test
EF Relational alignment is fixed at 10.0.14. Two backend tests pass with a
warning-free build and clean audits. Isolated Testcontainers test verifies
the migration and EF write/read through separate contexts. Owner confirms
the same local marker row survives Compose container recreation retaining
the named volume. Setup/troubleshooting docs cover the verified workflow.
No public probe endpoint or football-domain tables were introduced.
Only owner checkpoint review, commit, push, and publication verification remain.

## Project placement
In Progress. Implementation verified; awaiting owner checkpoint commit/push.

## Repository context
- [Roadmap](https://github.com/Ravynnox/Calliqo/blob/main/docs/ROADMAP.md)
- [Development state](https://github.com/Ravynnox/Calliqo/blob/main/docs/DEVELOPMENT_STATE.md)
- [Working instructions](https://github.com/Ravynnox/Calliqo/blob/main/AGENTS.md)

Snapshot: 2026-11-01. Local documentation changes must be committed and pushed by the owner before GitHub links reflect them.

## Completion record
Record actual checks, results, remaining gaps, and the owner-created commit/PR links before closing. For historical completed tasks, verify the cited commits and close as completed after import. Do not infer completion from a merged partial scaffold.

