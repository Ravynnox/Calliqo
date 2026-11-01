# Architecture

## Status
Approved foundation direction. React/Vite and .NET API scaffolds with
baseline frontend and backend tests are implemented in checkpoint c06c03d.
Local PostgreSQL/EF persistence and API health checks are implemented and
verified under TECH-002, awaiting its checkpoint commit. Football modules
and authentication remain planned.
Specific provider and library choices remain open where noted.

## System
React browser application → same-origin HTTPS entry point →
ASP.NET Core versioned REST API → PostgreSQL.

The backend is a modular monolith with initial modules:
Identity; Organizations and Teams; Football; Playbooks; Games.

Start with one backend project and one application DbContext.
Separate endpoints, application operations, domain rules, and persistence
within module boundaries.

## Access boundaries
Organizations provide administration and future subscription ownership.
Teams own football data.
Verify membership, explicit permission, and resource ownership server-side.
Use database constraints, scoped queries, write validation, and tests
to protect team boundaries.

## Playbooks
Store searchable metadata relationally and validated diagram documents
in PostgreSQL jsonb.
Use stable object identifiers, normalized coordinates, schema versions,
and immutable play revisions.
Games reference the play revision actually called.
SVG is provisionally preferred; a comparison with Konva precedes selection.

## Games
Record stable event identifiers, transactional sequence numbers,
participants, situation, outcomes, and audit information.
Prevent duplicate submissions and detect conflicting edits.
Corrections retain history.
Statistics derive from effective event revisions.

## Authentication
Evaluate ASP.NET Core Identity and an established OpenID Connect provider.
Secure browser sessions and CSRF protection are required if cookies are used.
External API authentication will use appropriate standards when introduced.

## Cloud and infrastructure
TECH-002 implements local PostgreSQL 18.6 in Docker Compose with host-run
frontend/API, loopback database access, and a named data volume. Startup,
API connectivity, EF migrations/write/read, and data retention across
container recreation are verified. See DATABASE_SETUP.md and PERSISTENCE_SETUP.md.
One scoped application DbContext uses Npgsql; an infrastructure-only probe
table verifies persistence without introducing football-domain models.
Use containers and managed services where justified.
Select the cloud provider and infrastructure-as-code tool through an ADR.
Provision the first cloud environment through infrastructure as code.
Plan protected state, secret management, environment separation,
deployment, migrations, backups, and verified recovery.

## Observability
Built-in /health/live and database /health/ready endpoints are implemented.
Outage/recovery checks confirm 200/503 behavior and subsequent readiness recovery.
Instrument the API with OpenTelemetry.
Plan correlated logs, metrics, traces, Grafana dashboards, and alerts.
Monitor availability, latency, errors, database behavior, and event saves.
Select telemetry storage and managed versus self-hosted operation
during cloud evaluation.

## Deferred capabilities
Private image storage and manual tracing; scouting; optional backend AI;
billing; external integrations; advanced analytics; offline synchronization.
