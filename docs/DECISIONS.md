# Architecture and Development Decisions

## ADR-001 — Temporary scoped minimatch override

- Date: 2026-11-01
- Status: Implemented by the owner; current local workflow checks passed.
- Related task: [TECH-001 (#5)](https://github.com/Ravynnox/Calliqo/issues/5)

### Context
Full npm audit reported 12 moderate affected-package findings rooted in
minimatch 10.2.6 (GHSA-g3hv-75wg-m249). Current eslint 10.14.0,
@eslint/config-array 0.23.5, and typescript-estree 8.72.1 still request
10.x; a normal latest-version update does not reach patched 11.0.1.
Production-only audit reported zero findings. npm's forced remediation
proposed toolchain downgrades that were not selected.

### Decision
The owner applied exact minimatch 11.0.1 overrides scoped to eslint,
@eslint/config-array, and @typescript-eslint/typescript-estree in
client/package.json, retaining the updated lockfile.

### Evidence
Owner output confirms clean install, dependency resolution, zero full audit
findings, frontend build/lint, and explicit source/config lint checks.
The agent's stdin-only unused-variable probe returned the expected
@typescript-eslint/no-unused-vars error without creating a source file.

### Consequences and review
Overrides cross upstream declared major-version ranges. Passing local
checks does not establish upstream support or all matching/API behavior.
Revalidate when lint configuration or tooling changes. Remove the overrides
when compatible upstream dependencies include the patch, then repeat the
clean install, audit, build, and lint checks. No acceptance of an unresolved
vulnerability is recorded. Baseline tests and the scaffold commit remain pending.

Reference: https://github.com/advisories/GHSA-g3hv-75wg-m249
