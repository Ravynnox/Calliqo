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
vulnerability is recorded. Baseline tests and the scaffold checkpoint were
subsequently completed/published in c06c03d. The override remains in the
manifest and lockfile at this review; removal requires a supported upstream update.

Reference: https://github.com/advisories/GHSA-g3hv-75wg-m249

## ADR-002 — Managed identity and browser sessions
- Date: 2026-11-06.
- Status: Accepted by owner; documentation publication pending under AUTH-001 (#9).
- Selected: Auth0 Universal Login through backend OIDC Code + PKCE, secure app
  cookies, app-owned memberships/permissions, public coach signup, verified-email
  organization/team creation, and initial eight-hour absolute application sessions.
- [Accepted decision](decisions/ADR-002-identity.md) compares alternatives and
  records recovery, invitations, future MFA, privacy, cost and production gates.
- AUTH-002 implementation is not started. No provider tenant or paid plan created.
