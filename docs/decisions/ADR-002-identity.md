# ADR-002 — Managed identity and browser sessions

- Date: 2026-11-06.
- Task: AUTH-001 ([#9](https://github.com/Ravynnox/Calliqo/issues/9)).
- Status: Accepted by owner on 2026-11-06; documentation publication pending.
- No auth source, package, migration, service account, or provider tenant has been created.

## Owner-confirmed requirements
- Minimize account-management operations using a managed sign-in provider.
- Any coach can register and create an organization/team in the initial release.
- Calliqo remains a React/.NET modular monolith with PostgreSQL; cloud host is undecided.
- Team membership/permissions are enforced server-side. Public registration grants
  no access to another coach's organization/team.

## Accepted decision
Use Auth0 Universal Login through standard OpenID Connect (OIDC). ASP.NET Core
handles Authorization Code flow with PKCE and issues Calliqo's secure application
session cookie. React calls its own backend using that cookie. Provider credentials
and any upstream tokens remain server-side. The owner approved this provider
and browser-session direction on 2026-11-06. Implementation belongs to AUTH-002.

OIDC specifies how Calliqo verifies a sign-in performed by a provider. It does
not define Calliqo's organization/team authorization. Provider choice does not
require hosting Calliqo in the same vendor's cloud.

## Options evaluated
| Option | Account operations | Cost considerations | Assessment for owner requirements |
|---|---|---|---|
| ASP.NET Core Identity + EF/Npgsql | Calliqo operates credential storage, confirmation/recovery email, MFA enrollment, patching and account support | .NET has no licensing charge; infrastructure, email, security and support work remain | Established and valid, but higher owner operations than requested |
| Auth0 managed OIDC | Provider runs identity service and hosted login; Calliqo configures flows and owns app sessions/authorization | Free and paid plans; MAU/rate limits, MFA, environments and advanced features are plan-dependent | Selected by owner; production provider-plan fit remains to be confirmed |
| Microsoft Entra External ID | Managed customer directory with self-service signup/reset and OIDC applications | MAU-based/provider-plan pricing and optional feature costs; check exact required flows | Credible managed alternative, particularly if Microsoft identity administration is preferred; it does not mandate Azure app hosting |
| Self-hosted Keycloak | Established OIDC server, but owner operates deployment, updates, database, TLS, availability and recovery | Hosting and operator time remain even without a managed-provider bill | Adds operational workload contrary to current preference |

Assessment is project judgment informed by the sources below, not a universal
ranking or an uptime/support guarantee. Standard OIDC reduces protocol coupling;
user/credential migration, MFA enrollment and provider configuration still make
future provider changes nontrivial.

## Cost snapshot and unresolved production gates
Auth0's public pricing page checked 2026-11-06 advertises a Free option up to
25,000 monthly active users. The rendered default paid example lists Essentials
at $35/month for up to 500 MAU with Pro MFA. Its B2C/B2B selections and feature
limits affect the applicable plan. Those numbers are a page snapshot, NOT a
Calliqo production quote or a commitment to buy a subscription. Extra features,
custom domains, email delivery, environments, support and MFA must be checked
against the actual selected plan/use case before production.

Calliqo organizations/teams are app-owned records, not automatically Auth0
Organizations. The displayed provider organization limit is not an architectural
limit on Calliqo's team table. Do not infer that avoiding that provider feature
bypasses any plan/use-case terms. Verify commercial fit separately.

No paid subscription is authorized. Before production confirm expected MAU,
monthly budget, provider region, email delivery requirements, MFA factors,
separate development/production identity environments, support needs, and
applicable limits. Revisit the provider if required features/cost do not fit.

## Accepted signup, recovery and onboarding direction
1. Coach selects Sign up/Sign in and is redirected to hosted login.
2. Provider handles authentication and account verification/recovery. Initial
   method is email/password; social methods can be added deliberately.
3. Backend validates issuer, signature, audience, state, nonce and PKCE/callback
   correlation using the supported OIDC handler, then creates/updates a minimal
   internal user and establishes its application session.
4. Require verified email before creating an organization/team or accepting
   email-targeted invitations. Unverified users need an explicit verification path.
5. An authenticated coach explicitly requests organization/team creation.
   The server assigns the creator the appropriate owner membership in a
   transaction; never accept an elevated role supplied by the browser.
6. Organization/team data and entitlements remain in PostgreSQL. Initial
   entitlement behavior follows AUTH-002; signup does not introduce billing.
7. Authenticated authorized owners/admins may invite coaches. Invitations need
   expiring, single-use tokens, intended verified email, organization/team and
   permitted role; the backend rechecks authority at acceptance. Creating a
   provider account alone does not redeem an invitation or add membership.

Password resets belong to the provider for provider-managed accounts. Calliqo
must define its own session invalidation/re-authentication behavior after account
security events; provider reset/logout does not by itself prove local cookies
are invalidated. No passwords are stored by Calliqo under this decision.

## Accepted session and browser model
- Same-origin React/backend entry point in deployment. Local development must
  deliberately align HTTPS and proxy/callback URLs when implemented in AUTH-002.
- Secure, HttpOnly application cookie, restrictive scope and appropriate SameSite
  policy. Use an eight-hour absolute application session for the initial
  version with no persistent remember-me option, as approved by the owner.
- Keep any provider tokens server-side. Browser localStorage/sessionStorage do
  not hold provider bearer/refresh tokens under this design.
- Validate anti-forgery tokens on state-changing cookie-authenticated app requests,
  including organization creation and logout. SameSite/CORS are not substitutes.
  OIDC protocol callback correlation remains the handler's responsibility.
- Session invalidation/revocation must be defined and tested in AUTH-002, including
  logout and disabled accounts. Resolve cookie-ticket/session storage there.
- Login is an explicit redirect flow. Normal API calls return 401/403 for missing
  authentication/permission; they must not return a hosted-login HTML redirect.
- Validate local return URLs and provider allowlisted callbacks/logout URLs.
- Persist/protect ASP.NET Core Data Protection keys across restarts and share them
  across instances in one environment; isolate application keys between environments.
  Select the cloud key-store mechanism with hosting/IaC, not now.

## Authorization, identifiers and future MFA/API clients
- Keep an internal stable user ID. With OIDC, uniquely map validated issuer + sub
  to that user. Email can change and is not the global identity key. Never merge
  different external accounts merely because email strings match.
- Query app-owned organization/team memberships and permissions server-side.
  Removing membership must affect access without waiting for a provider token's
  stale role claims to expire. Provider-wide roles are not team ownership.
- Keep MFA supported in the selected provider/plan. Define enrollment, recovery,
  admin policy and step-up verification before enabling it; do not assume a
  generic MFA label covers the required factors or that all factors are free.
- Future native/mobile or independent API clients use separately designed standard
  OAuth/OIDC client flows/scopes. Do not invent a JWT issuer or treat built-in
  Identity API proprietary bearer tokens as an OAuth/OIDC authorization server.
- Exclude passwords, cookies, authorization headers, reset/invite tokens, auth
  codes and unnecessary personal data from logs/traces. Recheck auth callback
  telemetry and keep identity-library PII debugging disabled in deployed systems.

## AUTH-002 validation plan
- Real development-provider round trip verifies callback/issuer/claims/signout.
  Automated app authorization tests use an isolated test scheme and disposable
  PostgreSQL, with no production provider credentials in CI.
- Unauthenticated API requests get 401; prohibited actions follow a defined
  403/404 disclosure policy; failed CSRF validation is rejected.
- Public signup cannot access existing teams; creation assigns membership only
  to its authenticated creator. Test two independent organizations/teams.
- Invite expiry, replay, intended recipient and elevated-role tampering fail safely.
- First real organization/team request demonstrates validation Problem Details
  and traceId, preserving the requirement transferred from TECH-003 to AUTH-002.
- Test session expiry/logout/revocation and membership removal. Plan recovery/MFA
  validation with the chosen provider features before release.

## Approval and Git next step
Owner approved Auth0 and this backend-cookie direction on 2026-11-06 in chat.
This accepts the decision; it does not create a provider tenant, authorize a
paid subscription, or implement authentication. Production plan/budget and
operational gates above remain applicable.

Owner reviews/stages the decision and related documentation, checks the staged
diff, then commits/pushes an AUTH-001 checkpoint referencing #9. Keep AUTH-001
open/In Progress until publication is verified. Starting AUTH-002 is a separate
development task requiring owner approval; deliver its full setup/code/validation
walkthrough directly in chat.
## Primary sources checked 2026-11-06
- [ASP.NET Core Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-10.0)
- [.NET licensing costs](https://dotnet.microsoft.com/en-us/platform/free)
- [ASP.NET Core browser Identity APIs](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity-api-authorization?view=aspnetcore-10.0)
- [OIDC code flow, PKCE and backend sessions](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/configure-oidc-web-authentication?view=aspnetcore-10.0)
- [CSRF protection](https://learn.microsoft.com/en-us/aspnet/core/security/anti-request-forgery?view=aspnetcore-10.0)
- [Data Protection configuration](https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/configuration/overview?view=aspnetcore-10.0)
- [Auth0 hosted login](https://auth0.com/docs/authenticate/login/auth0-universal-login)
- [Auth0 password recovery](https://auth0.com/docs/authenticate/database-connections/password-change)
- [Auth0 MFA](https://auth0.com/docs/secure/multi-factor-authentication)
- [Auth0 pricing](https://auth0.com/pricing)
- [Entra External ID](https://learn.microsoft.com/en-us/entra/external-id/customers/overview-customers-ciam)
- [Keycloak production operations](https://www.keycloak.org/server/configuration-production)
- [OIDC identifier stability](https://openid.net/specs/openid-connect-core-1_0.html#ClaimStabilityAndUniqueness)
