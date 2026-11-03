# AUTH-002 — Sign-in and teams

GitHub issue: [#10](https://github.com/Ravynnox/Calliqo/issues/10)

## Objective
Provide secure sign-in and authorized team access.

## Roadmap status
Planned. See docs/WORK_TRACKING.md for the issue mapping and latest evidence.

## Dependencies
[AUTH-001 (#9)](https://github.com/Ravynnox/Calliqo/issues/9), [TECH-003 (#7)](https://github.com/Ravynnox/Calliqo/issues/7)

## Acceptance criteria
- [ ] Coaches sign in and select only authorized teams.
- [ ] Organizations, memberships, explicit permissions, and team ownership are enforced.
- [ ] Initial subscription entitlements assume authorized teams are subscribed without payment integration.
- [ ] Critical authentication and authorization paths are tested.
- [ ] First real sign-in/team request endpoint validates input: valid requests succeed; malformed/invalid fields return 400 Problem Details with field-specific errors and traceId; contract tests cover both outcomes (transferred from TECH-003 #7 on 2026-11-02).

## Evidence and remaining work
Planned only. No implementation or verification is claimed. Request-validation demonstration was transferred from TECH-003 (#7) on 2026-11-02; the weather exercise is cancelled. If endpoint sequencing changes, explicitly transfer this criterion to the first real write endpoint before closing AUTH-002.

## Project placement
Backlog. Move to Ready only after dependencies and owner task approval.

## Repository context
- [Roadmap](https://github.com/Ravynnox/Calliqo/blob/main/docs/ROADMAP.md)
- [Development state](https://github.com/Ravynnox/Calliqo/blob/main/docs/DEVELOPMENT_STATE.md)
- [Working instructions](https://github.com/Ravynnox/Calliqo/blob/main/AGENTS.md)

Snapshot: 2026-11-01. Local documentation changes must be committed and pushed by the owner before GitHub links reflect them.

## Completion record
Record actual checks, results, remaining gaps, and the owner-created commit/PR links before closing. For historical completed tasks, verify the cited commits and close as completed after import. Do not infer completion from a merged partial scaffold.

