# Development State

## NEXT SESSION START HERE
Read AGENTS.md, this file, ROADMAP.md, and ARCHITECTURE.md.
Inspect the filesystem and Git state before making claims about progress.
Maintain documentation automatically. The owner controls code and Git
actions; non-documentation changes require explicit authorization.

## Current work
- Phase: 0 — Project Foundation
- Milestone: Repository instructions and development memory
- Task: FND-002 — Establish repository memory
- Status: In progress
- Last completed task: FND-001 — Foundation review approved
- Last update: 2026-11-01

## Approved direction
- Mobile-first web application.
- Modular monolith using the required React and .NET stack.
- Structured and versioned play data.
- Event-based game tracking with audited corrections.
- Server-enforced team isolation.
- Cloud hosting with infrastructure as code.
- OpenTelemetry instrumentation and Grafana dashboards and alerts.

## Work completed
- Foundation proposal reviewed and approved.
- Cloud, infrastructure-as-code, and Grafana requirements accepted.
- Local tool versions inspected.
- Explicit authorization received to create the five initial documents:
  AGENTS.md, README.md, docs/DEVELOPMENT_STATE.md, docs/ROADMAP.md,
  and docs/ARCHITECTURE.md.
- Initial documentation files created; application scaffolding has not begun.
- Owner created the root .gitignore and initialized Git on main.
- Ignore rules verified for frontend dependencies, .NET build output,
  and local environment configuration.
- Owner authorized automatic documentation creation and updates.
- AGENTS.md and README.md updated to reflect the revised working rules.
- Owner created the initial foundation commit, verified as 4b9bb9f.

## Files created
- AGENTS.md
- README.md
- docs/DEVELOPMENT_STATE.md
- docs/ROADMAP.md
- docs/ARCHITECTURE.md
- .gitignore (created by the owner)

## Documentation modified in this step
- docs/DEVELOPMENT_STATE.md

## Verification
Observed versions:
Git 2.56.0.windows.1; .NET SDK 10.0.201; Node.js 20.20.1;
npm 10.8.2; Docker 29.9.2; Docker Compose v5.6.1.

Application builds, tests, Docker engine operation, and second-computer
setup have not been verified.

All five initial documents were read back after creation and reviewed
for scope, task status, architecture, and cloud/observability requirements.
The root .gitignore and five initial documents are present.
Git history confirms the six foundation files were committed on main.
The working tree was clean before this development-state update.
Ignore checks passed for client/node_modules/example.js,
server/Calliqo.Api/bin/example.dll, and .env.

## Repository status
- Git is initialized on main.
- Last verified commit: 4b9bb9f — FND-002: establish project founcdational docs.
- The foundation files are committed locally.
- This development-state update is not yet committed.
- No remote is configured.
- Owner-provided target remote: https://github.com/Ravynnox/Calliqo.git.
  Remote existence, access, and contents have not yet been verified.
- No push has been performed or verified.
- Cross-computer synchronization has not been established.

## Work remaining
- Owner reviews and commits this development-state update to finalize FND-002.
- Configure the intended GitHub remote and push the committed documentation.
- Verify retrieval on the second computer.
- Add remaining foundation reference documents.

## Open decisions
Identity approach; graphics renderer; cloud provider; infrastructure
tool; telemetry hosting and storage; initial football rules;
supported browser/device matrix.

## Next recommended action
Owner reviews and commits this state update, configures the remote,
checks for existing remote branches, and pushes if the histories permit.
Verify remote synchronization before second-computer setup.
Create application directories as their implementation tasks begin.

## Completion rule
FND-002 remains in progress until its acceptance criteria are verified.
Update this record to reflect actual file and Git changes before handoff.
