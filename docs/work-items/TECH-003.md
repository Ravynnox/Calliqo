# TECH-003 — API conventions and CI

GitHub issue: [#7](https://github.com/Ravynnox/Calliqo/issues/7)

## Objective
Establish consistent API contracts and automated build checks.

## Roadmap status
Complete. Implementation and documentation published in a77b651; CI passed.

## Dependencies
[TECH-002 (#6)](https://github.com/Ravynnox/Calliqo/issues/6)

## Acceptance criteria
- [x] Versioned API conventions and OpenAPI contracts are documented locally.
- [x] Standardized error responses are demonstrated by the unknown-route test.
- [x] GitHub Actions runs required build, lint, and applicable tests.
- [x] Workflow uses no application secrets; no application secret values were observed in reviewed changes or CI evidence.
- [x] Owner publishes implementation and associated documentation.

## Scope decision — 2026-11-02
The proposed weather days-query validation exercise is cancelled as unnecessary
scaffold work, not marked implemented or tested. No standalone issue exists
for it. Request-validation demonstration and tests are transferred to
[AUTH-002 (#10)](https://github.com/Ravynnox/Calliqo/issues/10), on the first
real sign-in/team request endpoint. TECH-003 may complete once its remaining
CI and publication criteria pass; AUTH-002 must verify the transferred work.

## Evidence and remaining work
Owner published a77b651b655bc3301e8b77549b3c77ec179efe42. Local HEAD,
origin/main, and live GitHub main match; working tree was clean at verification.
[CI run 41551585014](https://github.com/Ravynnox/Calliqo/actions/runs/41551585014)
succeeded for that exact commit on a main push. Frontend install, lint, one
Vitest test, and production build passed. Backend restore, warning-free Release
build, and all four tests passed (zero failed/skipped), including the isolated
PostgreSQL persistence test. Docker verification passed. SDK 10.0.204 follows
global.json latestPatch policy; Node 26.13.0 and npm 11.20.0 match project inputs.
The workflow needs no application secrets or local environment files.
No local tests were rerun by Codex; these are verified remote results.
Real request validation remains under AUTH-002 (#10). Branch protection,
PR-trigger execution, deployment, and second-computer retrieval are not verified
by this successful main-push run.

## Project placement
Done after reconciling issue and Project state with the verified evidence.
## Repository context
- [Roadmap](https://github.com/Ravynnox/Calliqo/blob/main/docs/ROADMAP.md)
- [Development state](https://github.com/Ravynnox/Calliqo/blob/main/docs/DEVELOPMENT_STATE.md)
- [Working instructions](https://github.com/Ravynnox/Calliqo/blob/main/AGENTS.md)

Snapshot: 2026-11-02. Local documentation must be committed and pushed by the
owner before repository file links reflect it. Issue-body publication does not
publish repository files.

## Completion record
Implementation and associated documentation are committed/published in a77b651.
CI succeeds for that commit. Post-run completion notes are local and should
join the next meaningful owner checkpoint; no additional code or Git mutation
was performed by Codex. Cancelled weather validation is not counted as done.
