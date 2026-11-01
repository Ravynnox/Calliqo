# Development State

## NEXT SESSION START HERE
Read AGENTS.md, this file, ROADMAP.md, and ARCHITECTURE.md.
Inspect the filesystem and Git state before making claims about progress.
Maintain documentation automatically. The owner controls code and Git
actions; non-documentation changes require explicit authorization.

## Current work
- Phase: 0 — Project Foundation
- Milestone: Two-computer continuity
- Task: FND-003 — Two-computer setup
- Status: In progress
- Last completed task: FND-002 — Establish repository memory
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
- Owner committed the handoff update as 3a5d1e5 and configured origin.
- Live read-only GitHub verification confirmed remote main matches local
  commit 3a5d1e58c394a04fd516c86c8c6aa72e460b3e11.
- Initial FND-002 documents are committed and published.
- Added docs/SETUP.md with the second-computer verification procedure.

## Files created
- AGENTS.md
- README.md
- docs/DEVELOPMENT_STATE.md
- docs/ROADMAP.md
- docs/ARCHITECTURE.md
- .gitignore (created by the owner)

## Documentation modified in this step
- docs/DEVELOPMENT_STATE.md
- docs/ROADMAP.md
- docs/SETUP.md (new)

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
- Last verified commit: 3a5d1e5 — docs: record foundation progress and target remote.
- Origin: https://github.com/Ravynnox/Calliqo.git.
- Local main, origin/main, and live GitHub main matched at verification.
- The working tree was clean before this documentation update.
- This setup guide and status/roadmap update are not yet committed or pushed.
- Retrieval and environment verification on the second computer remain pending.

## Work remaining
- Owner reviews, commits, and pushes the FND-003 guide and status update.
- Follow docs/SETUP.md on the second computer and report commit/tool results.
- Add remaining foundation reference documents.

## Open decisions
Identity approach; graphics renderer; cloud provider; infrastructure
tool; telemetry hosting and storage; initial football rules;
supported browser/device matrix.

## Next recommended action
Owner commits and pushes the three documentation files listed above.
Retrieve that resulting commit on the second computer and follow docs/SETUP.md.
Create application directories as their implementation tasks begin.

## Completion rule
FND-002 foundation artifacts are committed and published.
FND-003 remains in progress until both-computer verification is complete.
The current milestone is not finalized until its documentation is committed.
Update this record to reflect actual file and Git changes before handoff.
