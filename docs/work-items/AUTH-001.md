# AUTH-001 — Identity decision

GitHub issue: [#9](https://github.com/Ravynnox/Calliqo/issues/9)

## Objective
Choose an established authentication approach before implementing accounts.

## Roadmap status
In progress. Owner accepted ADR-002 on 2026-11-06; decision publication remains.

## Dependencies
[FND-001 (#1)](https://github.com/Ravynnox/Calliqo/issues/1)

## Acceptance criteria
- [x] Accepted ADR compares ASP.NET Core Identity, Auth0, Entra External ID and Keycloak.
- [x] Accepted ADR addresses public signup/provisioning, recovery, invitations, sessions and future MFA.
- [x] Accepted ADR evaluates browser security, costs/plan gates, operations and future API clients.
- [x] Owner approves the selected approach before implementation.
- [ ] Approved decision and associated documentation are committed/published by the owner.

## Evidence and remaining work
Owner accepted Auth0 Universal Login with backend OIDC Code + PKCE and secure
application cookies on 2026-11-06. Public coach signup, verified-email organization/
team creation, app-owned memberships, eight-hour initial sessions, and the
remaining browser/recovery/invitation/MFA direction are recorded in accepted
ADR-002 at docs/decisions/ADR-002-identity.md. Alternatives and primary sources
were reviewed. Production plan/features/budget remain explicit gates; no paid
subscription, provider tenant, package, or auth implementation was created.
Owner publication of the decision/docs is the only remaining AUTH-001 checkpoint.

## Project placement
In Progress pending verified decision/documentation publication. Keep open until
that owner-created checkpoint is published; do not start AUTH-002 automatically.
## Repository context
- [Roadmap](https://github.com/Ravynnox/Calliqo/blob/main/docs/ROADMAP.md)
- [Development state](https://github.com/Ravynnox/Calliqo/blob/main/docs/DEVELOPMENT_STATE.md)
- [Working instructions](https://github.com/Ravynnox/Calliqo/blob/main/AGENTS.md)

Snapshot: 2026-11-06. Local documentation changes must be committed and pushed by the owner before GitHub links reflect them.

## Completion record
Record actual checks, results, remaining gaps, and the owner-created commit/PR links before closing. For historical completed tasks, verify the cited commits and close as completed after import. Do not infer completion from a merged partial scaffold.

