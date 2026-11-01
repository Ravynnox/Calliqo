# Roadmap

Statuses: Planned, In progress, Complete.
Complete requires verified acceptance criteria and committed documentation.

| Task | Dependency | Deliverable / acceptance | Status |
|---|---|---|---|
| FND-001 Foundation review | None | Approve MVP and architectural direction | Complete |
| FND-002 Repository memory | FND-001 | Approved instructions and state documents committed | Complete |
| FND-003 Two-computer setup | FND-002 | Document tools; both computers retrieve matching commit | In progress |
| TECH-001 Application scaffolds | FND-003 | Frontend/backend build and baseline checks pass | Planned |
| TECH-002 API/database connection | TECH-001 | Compose startup, migrations, health and persistence verified | Planned |
| TECH-003 API conventions and CI | TECH-002 | Versioned API, OpenAPI, errors, validation and CI checks | Planned |
| OBS-001 Baseline telemetry | TECH-003 | Correlated request logs, metrics and traces verified | Planned |
| AUTH-001 Identity decision | FND-001 | Accepted identity ADR | Planned |
| AUTH-002 Sign-in and teams | AUTH-001, TECH-003 | Memberships, permissions and initial entitlements work | Planned |
| AUTH-003 Team isolation | AUTH-002 | Unauthorized reads, writes and relationships rejected | Planned |
| DOM-001 Football domain | AUTH-003 | Formats, rules, seasons and rosters validated | Planned |
| PLAY-001 Structured play contract | DOM-001 | Versioned schema and round-trip tests | Planned |
| PLAY-002 Playbook persistence | PLAY-001 | Save/reopen, duplication and stale-update protection | Planned |
| EDIT-001 Renderer evaluation | PLAY-001 | Device comparison and accepted renderer ADR | Planned |
| EDIT-002 Interactive editor | EDIT-001, PLAY-002 | Placement, routes, assignments, undo/redo and persistence | Planned |
| GAME-001 Event contract | PLAY-001 | Attribution, ordering, retries and correction rules | Planned |
| GAME-002 Game-day workflow | GAME-001, EDIT-002 | Record/retry events without duplication | Planned |
| GAME-003 Corrections/statistics | GAME-002 | Audited corrections and verified derived statistics | Planned |
| CLOUD-001 Hosting/IaC decision | TECH-003 | Provider, costs, tooling and telemetry ADR | Planned |
| INFRA-001 Cloud development | CLOUD-001, OBS-001 | IaC provisions environment; recreation verified | Planned |
| OBS-002 Grafana and alerts | INFRA-001 | Dashboards and controlled-failure alert test | Planned |
| INFRA-002 Production/recovery | INFRA-001 | Production IaC, deployment, migrations and restore verification | Planned |
| REL-001 MVP verification | GAME-003, OBS-002, INFRA-002 | End-to-end, isolation, device and operational criteria pass | Planned |

FND-002 evidence: foundation commit 4b9bb9f and handoff commit 3a5d1e5.
Live GitHub main matched local main at 3a5d1e5 on 2026-11-01.
FND-003 verification and its documentation commit remain outstanding.

Later phases: image import, scouting, recommendations, and commercial
enhancements. Detailed scope requires approval before implementation.
