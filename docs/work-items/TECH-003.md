# TECH-003 — API conventions and CI

GitHub issue: [#7](https://github.com/Ravynnox/Calliqo/issues/7)

## Objective
Establish consistent API contracts and automated build checks.

## Roadmap status
In progress. Foundation implementation is local; CI and publication remain.

## Dependencies
[TECH-002 (#6)](https://github.com/Ravynnox/Calliqo/issues/6)

## Acceptance criteria
- [x] Versioned API conventions and OpenAPI contracts are documented locally.
- [x] Standardized error responses are demonstrated by the unknown-route test.
- [ ] GitHub Actions runs required build, lint, and applicable tests.
- [ ] No secrets are committed or exposed in CI output.
- [ ] Owner publishes implementation and associated documentation.

## Scope decision — 2026-11-02
The proposed weather days-query validation exercise is cancelled as unnecessary
scaffold work, not marked implemented or tested. No standalone issue exists
for it. Request-validation demonstration and tests are transferred to
[AUTH-002 (#10)](https://github.com/Ravynnox/Calliqo/issues/10), on the first
real sign-in/team request endpoint. TECH-003 may complete once its remaining
CI and publication criteria pass; AUTH-002 must verify the transferred work.

## Evidence and remaining work
Owner implemented Problem Details, traceId, unknown-route test, /api/v1 routing,
and the OpenAPI contract test. Owner reports all four backend tests pass after
correcting caller URLs. Codex did not rerun tests. The weather validation
walkthrough is superseded in docs/API_VALIDATION.md. Retain existing foundation
coverage until real endpoints replace it. GitHub Actions, verified CI runs,
and publication remain outstanding. The owner CI walkthrough is prepared in
docs/CI_SETUP.md; owner created the workflow at .github/workflows/ci.yml.
Placement and contents are reviewed locally; publication and CI execution remain
pending. Do not close this issue.

## Project placement
In Progress. Next recommended work is CI setup, with owner controlling source,
configuration, and Git mutations.

## Repository context
- [Roadmap](https://github.com/Ravynnox/Calliqo/blob/main/docs/ROADMAP.md)
- [Development state](https://github.com/Ravynnox/Calliqo/blob/main/docs/DEVELOPMENT_STATE.md)
- [Working instructions](https://github.com/Ravynnox/Calliqo/blob/main/AGENTS.md)

Snapshot: 2026-11-02. Local documentation must be committed and pushed by the
owner before repository file links reflect it. Issue-body publication does not
publish repository files.

## Completion record
Keep open until the remaining acceptance criteria and owner-created checkpoint
are verified. Cancelled scaffold work must not be reported as completed work.
