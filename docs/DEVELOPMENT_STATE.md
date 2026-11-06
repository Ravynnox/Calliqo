# Development State

## Working rules
Read AGENTS.md, this file, ROADMAP.md, ARCHITECTURE.md, DECISIONS.md,
WORK_TRACKING.md, and the active issue before work. Codex may maintain docs;
owner controls source/configuration/infrastructure and Git changes. Present
all actionable walkthroughs directly in chat for every task/document. Never
read/display actual .env or User Secrets unnecessarily.

## Current state — reviewed 2026-11-06
- OBS-001 acceptance is verified: published faddf7d and successful CI.
- Issue #8 is closed and Project card Done; both confirmed by remote read-back.
- Current task: AUTH-001 — Identity decision (#9), owner approved starting on 2026-11-06.
- Owner prefers managed sign-in and public coach registration/organization-team creation.
- Owner accepted ADR-002 on 2026-11-06: Auth0 hosted login, backend OIDC/PKCE,
  secure app cookies, eight-hour sessions and app-owned memberships. Decision
  publication is pending; AUTH-002 implementation has not started.
- FND-003 second-computer verification remains explicitly deferred.

## Published foundation
- TECH-001 c06c03d: React/TypeScript/Vite and .NET 10 scaffolds, tool pins,
  baseline tests. Scoped minimatch 11.0.1 override remains under ADR-001.
- TECH-002 05e1abd: PostgreSQL Compose, private local connection configuration,
  EF/Npgsql context and probe migration, health separation, isolated persistence
  test, and owner-verified named-volume durability.
- TECH-003 a77b651: /api/v1, OpenAPI/error contract tests, and GitHub Actions.
  Run 41551585014 passed one frontend/four backend tests for that checkpoint.
- OBS-001 faddf7db0f72b49432fe872594142c640411a48e: local LGTM 0.39.0 with optional
  Compose profile and loopback ports 3000/4318; OpenTelemetry 1.20.1 packages;
  opt-in OTLP export; JSON request summaries; Activity trace-ID error correlation;
  native Npgsql traces with SQL/data-source tag cleanup; SELECT 1 readiness;
  incoming trace-ID and isolated readiness HTTP tests.

## Verified OBS-001 evidence
- https://github.com/Ravynnox/Calliqo/actions/runs/41898883349 passed both jobs
  for faddf7d. Remote logs confirm one frontend test and six backend tests passed,
  zero failed/skipped, and zero backend build warnings/errors. Lint/build passed.
- Local HEAD, origin/main, and live GitHub main matched faddf7d; working tree was
  clean before writing these post-run completion notes.
- Owner reports manual success: matching HTTP/database/log traces, latency/error
  metrics, database outage live 200/ready 503 and recovery 200, LGTM outage while
  API remains responsive at 200, and fresh exports after LGTM recovery without
  API restart. Owner also reports HTTP marker/driver-event privacy checks passed.
- Codex did not independently execute those manual checks or inspect Grafana.
- Tag cleanup does not automatically redact arbitrary future exception events.
  Reassess telemetry attributes/events as application features are introduced.
- Application secrets are not needed by CI. .env remains local/ignored; API
  configuration does not automatically read Compose .env.

## Deliberately pending
- AUTH-001 decision is accepted in decisions/ADR-002-identity.md before AUTH-002 sign-in/team
  implementation. Real request validation is tracked in AUTH-002 (#10); the
  weather days-query exercise is cancelled and must not be recreated.
- Remaining required frontend libraries and football modules follow their roadmap
  tasks; their absence is not a missed foundation walkthrough.
- Cloud provider, IaC tool/state protection, production telemetry storage/hosting,
  Grafana dashboards/alerts, backups/restore and renderer remain open. Identity
  provider/design is selected; production plan/operational fit remains a gate.
- PersistenceProbe is temporary. Remove via migration before MVP once real module
  persistence coverage replaces it. Do not rewrite published migrations.
- Local PostgreSQL bootstrap role is privileged; production requires separate
  runtime and migration/provisioning permissions.
- Main-push CI success does not verify PR-trigger behavior, required merge checks,
  production readiness, or second-computer retrieval.

## Documentation and Git
- Owner published all implementation and associated reference docs in faddf7d.
- Completion notes written after CI remain local; include them with the next
  meaningful owner-reviewed checkpoint. Do not require a status-only commit.
- Prior repository-wide review checked 87 files, including 42 Markdown documents,
  and corrected stale status summaries. Historical walkthroughs are reference.
- No source/configuration/Git changes were performed by Codex for completion.
- Next action: owner stages/reviews and commits/pushes the accepted decision
  and related docs. Verify publication before closing #9; then seek approval
  for AUTH-002 and provide its full walkthrough directly in chat.
  AUTH-002 is not started; no provider account, paid plan, code or config created.

## AUTH-001 tracking verification — 2026-11-06
GitHub issue #9 is open, its body matches the local snapshot, and its Project
card is In Progress. Draft ADR-002 is local; source/configuration and Git were
not changed. No tests are needed for the decision/documentation-only work;
whitespace checks passed at review. Owner has since approved provider/design;
decision publication remains pending.
