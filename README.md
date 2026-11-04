# Calliqo

A mobile-first American football coaching web application.

## Initial MVP
- Secure coach sign-in and team-specific access.
- Structured, editable football playbooks.
- Game-event recording, corrections, and basic statistics.

## Development status
Technical foundation: React/TypeScript frontend and .NET API scaffolds are
implemented and tested. Local PostgreSQL, EF migrations/persistence, health
checks, and isolated integration tests are verified and published in TECH-002
checkpoint 05e1abd. TECH-003 API conventions and CI are published in a77b651;
both CI jobs passed. OBS-001 is in progress: local Grafana and package setup
are complete; API telemetry wiring is still pending.
Football features remain planned.

## Start here
- Working rules: AGENTS.md
- Current state: docs/DEVELOPMENT_STATE.md
- Development plan: docs/ROADMAP.md
- Architecture: docs/ARCHITECTURE.md
- GitHub work tracking: docs/WORK_TRACKING.md
- Local database: docs/DATABASE_SETUP.md
- Testing: docs/TESTING.md and docs/PERSISTENCE_TESTS.md
- API contracts: docs/API_CONTRACTS.md

Development is guided. Documentation is maintained automatically; the
owner controls code changes and Git actions. Non-documentation changes
require explicit authorization. Completed walkthroughs are reference material;
DEVELOPMENT_STATE.md identifies the current work.
