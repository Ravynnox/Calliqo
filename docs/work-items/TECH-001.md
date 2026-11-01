# TECH-001 — Application scaffolds

GitHub issue: [#5](https://github.com/Ravynnox/Calliqo/issues/5)

## Objective
Establish and verify the React/TypeScript frontend and .NET 10 API.

## Roadmap status
In progress. See docs/WORK_TRACKING.md for the issue mapping and latest evidence.

## Dependencies
[FND-003 (#3)](https://github.com/Ravynnox/Calliqo/issues/3); local setup proceeding with second-computer verification pending

## Acceptance criteria
- [x] Frontend production build, ESLint, and starter browser interaction pass.
- [x] Backend solution build and weatherforecast/OpenAPI endpoint checks pass.
- [x] Dependency conflicts and reported vulnerable packages are resolved and audited.
- [ ] Tool version requirements and baseline test setup are established and verified.
- [ ] Scaffold and associated documentation are reviewed, committed, and pushed by the owner.

## Evidence and remaining work
Frontend uses TypeScript 6.0.3 after template conflict; build/lint/browser passed. Backend references Microsoft.OpenApi 2.7.5; build, audit, and both endpoint checks passed. Version pinning and baseline tests remain. Scaffold is uncommitted. Work is temporarily paused for FND-004.

## Project placement
In Progress. Keep FND-003 visibly pending and TECH-001 waiting while FND-004 is active.

## Repository context
- [Roadmap](https://github.com/Ravynnox/Calliqo/blob/main/docs/ROADMAP.md)
- [Development state](https://github.com/Ravynnox/Calliqo/blob/main/docs/DEVELOPMENT_STATE.md)
- [Working instructions](https://github.com/Ravynnox/Calliqo/blob/main/AGENTS.md)

Snapshot: 2026-11-01. Local documentation changes must be committed and pushed by the owner before GitHub links reflect them.

## Completion record
Record actual checks, results, remaining gaps, and the owner-created commit/PR links before closing. For historical completed tasks, verify the cited commits and close as completed after import. Do not infer completion from a merged partial scaffold.

