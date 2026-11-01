# Roadmap

Statuses: Planned, In progress, Complete.
Complete requires verified acceptance criteria and committed documentation.

| Task | Dependency | Deliverable / acceptance | Status | Issue |
|---|---|---|---|---|
| FND-001 Foundation review | None | Approve MVP and architectural direction | Complete | [#1](https://github.com/Ravynnox/Calliqo/issues/1) |
| FND-002 Repository memory | FND-001 | Approved instructions and state documents committed | Complete | [#2](https://github.com/Ravynnox/Calliqo/issues/2) |
| FND-003 Two-computer setup | FND-002 | Document tools; both computers retrieve matching commit | In progress | [#3](https://github.com/Ravynnox/Calliqo/issues/3) |
| FND-004 GitHub work tracking | FND-002 | Publish roadmap issues, configure Project board, link issue IDs, document commit/PR workflow, and verify | Complete | [#4](https://github.com/Ravynnox/Calliqo/issues/4) |
| TECH-001 Application scaffolds | FND-003 (second-computer verification pending; local setup proceeding) | Frontend/backend build and baseline checks pass | In progress | [#5](https://github.com/Ravynnox/Calliqo/issues/5) |
| TECH-002 API/database connection | TECH-001 | Compose startup, migrations, health and persistence verified | Planned | [#6](https://github.com/Ravynnox/Calliqo/issues/6) |
| TECH-003 API conventions and CI | TECH-002 | Versioned API, OpenAPI, errors, validation and CI checks | Planned | [#7](https://github.com/Ravynnox/Calliqo/issues/7) |
| OBS-001 Baseline telemetry | TECH-003 | Correlated request logs, metrics and traces verified | Planned | [#8](https://github.com/Ravynnox/Calliqo/issues/8) |
| AUTH-001 Identity decision | FND-001 | Accepted identity ADR | Planned | [#9](https://github.com/Ravynnox/Calliqo/issues/9) |
| AUTH-002 Sign-in and teams | AUTH-001, TECH-003 | Memberships, permissions and initial entitlements work | Planned | [#10](https://github.com/Ravynnox/Calliqo/issues/10) |
| AUTH-003 Team isolation | AUTH-002 | Unauthorized reads, writes and relationships rejected | Planned | [#11](https://github.com/Ravynnox/Calliqo/issues/11) |
| DOM-001 Football domain | AUTH-003 | Formats, rules, seasons and rosters validated | Planned | [#12](https://github.com/Ravynnox/Calliqo/issues/12) |
| PLAY-001 Structured play contract | DOM-001 | Versioned schema and round-trip tests | Planned | [#13](https://github.com/Ravynnox/Calliqo/issues/13) |
| PLAY-002 Playbook persistence | PLAY-001 | Save/reopen, duplication and stale-update protection | Planned | [#14](https://github.com/Ravynnox/Calliqo/issues/14) |
| EDIT-001 Renderer evaluation | PLAY-001 | Device comparison and accepted renderer ADR | Planned | [#15](https://github.com/Ravynnox/Calliqo/issues/15) |
| EDIT-002 Interactive editor | EDIT-001, PLAY-002 | Placement, routes, assignments, undo/redo and persistence | Planned | [#16](https://github.com/Ravynnox/Calliqo/issues/16) |
| GAME-001 Event contract | PLAY-001 | Attribution, ordering, retries and correction rules | Planned | [#17](https://github.com/Ravynnox/Calliqo/issues/17) |
| GAME-002 Game-day workflow | GAME-001, EDIT-002 | Record/retry events without duplication | Planned | [#18](https://github.com/Ravynnox/Calliqo/issues/18) |
| GAME-003 Corrections/statistics | GAME-002 | Audited corrections and verified derived statistics | Planned | [#19](https://github.com/Ravynnox/Calliqo/issues/19) |
| CLOUD-001 Hosting/IaC decision | TECH-003 | Provider, costs, tooling and telemetry ADR | Planned | [#20](https://github.com/Ravynnox/Calliqo/issues/20) |
| INFRA-001 Cloud development | CLOUD-001, OBS-001 | IaC provisions environment; recreation verified | Planned | [#21](https://github.com/Ravynnox/Calliqo/issues/21) |
| OBS-002 Grafana and alerts | INFRA-001 | Dashboards and controlled-failure alert test | Planned | [#22](https://github.com/Ravynnox/Calliqo/issues/22) |
| INFRA-002 Production/recovery | INFRA-001 | Production IaC, deployment, migrations and restore verification | Planned | [#23](https://github.com/Ravynnox/Calliqo/issues/23) |
| REL-001 MVP verification | GAME-003, OBS-002, INFRA-002 | End-to-end, isolation, device and operational criteria pass | Planned | [#24](https://github.com/Ravynnox/Calliqo/issues/24) |

FND-002 evidence: foundation commit 4b9bb9f and handoff commit 3a5d1e5.
Live GitHub main matched local main at 3a5d1e5 on 2026-11-01.
FND-003 verification and its documentation commit remain outstanding.

Later phases: image import, scouting, recommendations, and commercial
enhancements. Detailed scope requires approval before implementation.

## GitHub tracking
The owner approved FND-004 before resuming TECH-001 version pinning.
See [work tracking](WORK_TRACKING.md) for the issue/body snapshot index,
publication procedure, Project statuses, and commit/PR conventions.
All 24 roadmap tasks have published GitHub issues linked above.
Project: [Calliqo Development](https://github.com/users/Ravynnox/projects/2).
Tracking documentation is published in d01167c. Workflow board columns are
verified as grouped by Status (verticalGroupByFields). FND-004 is complete;
TECH-001 resumes for version pinning and baseline tests.
