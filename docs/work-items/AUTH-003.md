# AUTH-003 — Team isolation

GitHub issue: [#11](https://github.com/Ravynnox/Calliqo/issues/11)

## Objective
Verify that team boundaries protect reads, writes, and relationships.

## Roadmap status
Planned. See docs/WORK_TRACKING.md for the issue mapping and latest evidence.

## Dependencies
[AUTH-002 (#10)](https://github.com/Ravynnox/Calliqo/issues/10)

## Acceptance criteria
- [ ] Integration tests cover unauthorized access with two distinct teams.
- [ ] Cross-team reads, inserts, updates, deletes, and relationship references are rejected.
- [ ] Authorized multi-team users operate within the selected authorized team.
- [ ] Isolation rules and applicable database constraints are documented.

## Evidence and remaining work
Planned only. No implementation or verification is claimed.

## Project placement
Backlog. Move to Ready only after dependencies and owner task approval.

## Repository context
- [Roadmap](https://github.com/Ravynnox/Calliqo/blob/main/docs/ROADMAP.md)
- [Development state](https://github.com/Ravynnox/Calliqo/blob/main/docs/DEVELOPMENT_STATE.md)
- [Working instructions](https://github.com/Ravynnox/Calliqo/blob/main/AGENTS.md)

Snapshot: 2026-11-01. Local documentation changes must be committed and pushed by the owner before GitHub links reflect them.

## Completion record
Record actual checks, results, remaining gaps, and the owner-created commit/PR links before closing. For historical completed tasks, verify the cited commits and close as completed after import. Do not infer completion from a merged partial scaffold.

