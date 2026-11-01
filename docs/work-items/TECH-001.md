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
- [x] Tool version requirements are established and verified.
- [x] Baseline test setup is established and verified.
- [ ] Scaffold and associated documentation are reviewed, committed, and pushed by the owner.

## Evidence and remaining work
Frontend uses TypeScript 6.0.3 after template conflict; build/lint/browser passed. Backend references Microsoft.OpenApi 2.7.5; build, NuGet audit, and both endpoint checks passed. Owner added and verified .NET/Node/npm requirements. Scoped minimatch 11.0.1 overrides resolve the tooling advisory: clean install, full audit (zero findings), build, lint, and explicit file lint checks passed per owner output. Agent stdin-only unused-variable probe produced the expected ESLint error. Temporary override rationale and limitations are recorded in docs/DECISIONS.md. Frontend and backend baseline tests now pass; owner checkpoint commit/push remains and scaffold/version/test files are uncommitted. FND-004 is complete.

## Project placement
In Progress. All scaffold/version/test checks pass; owner checkpoint commit/push remains. Second-computer verification remains pending under FND-003.

## Repository context
- [Roadmap](https://github.com/Ravynnox/Calliqo/blob/main/docs/ROADMAP.md)
- [Development state](https://github.com/Ravynnox/Calliqo/blob/main/docs/DEVELOPMENT_STATE.md)
- [Working instructions](https://github.com/Ravynnox/Calliqo/blob/main/AGENTS.md)

Snapshot: 2026-11-01. Local documentation changes must be committed and pushed by the owner before GitHub links reflect them.

## Completion record
Owner confirmed frontend test/build/lint/audit pass and supplied backend output:
one passing integration test (zero failed/skipped), successful solution build,
and no known vulnerable packages in API or test project from current sources.
Test class is WeatherForecastTests; filename is still UnitTest1.cs pending
owner rename. Scaffold/configuration/tests and updated docs are uncommitted.
Do not close issue #5 until the owner-created checkpoint and publication are verified.

Record actual checks, results, remaining gaps, and the owner-created commit/PR links before closing. For historical completed tasks, verify the cited commits and close as completed after import. Do not infer completion from a merged partial scaffold.

