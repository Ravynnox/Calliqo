# Development State

## Session startup and responsibilities
Read AGENTS.md, this file, ROADMAP.md, ARCHITECTURE.md, DECISIONS.md,
WORK_TRACKING.md, and the active work item. Inspect Git before reporting state.
Codex maintains documentation. Owner controls code/configuration/infrastructure
and Git changes. Never read/display actual .env or User Secrets unnecessarily.

## Current task
- Phase: Technical Foundation.
- Current: OBS-001 — Baseline telemetry, issue #8, In Progress.
- Last completed: TECH-003 — API conventions and CI, issue #7.
- Last review: 2026-11-04.
- Owner approved resuming observability on 2026-11-04 and explicitly requires
  the full walkthrough directly in chat for every task/document. This global
  communication requirement is now recorded in AGENTS.md.
- Owner implemented HTTP telemetry wiring and the fifth correlation test,
  confirmed by source inspection. Owner reports ready to continue. Codex has
  not independently rerun build/tests or inspected live telemetry this turn.

## Published and verified foundation
- TECH-001 c06c03d: React/TypeScript/Vite and .NET 10 scaffolds; tool pins;
  one frontend interaction test and backend baseline. Temporary scoped minimatch
  11.0.1 override remains documented in ADR-001.
- TECH-002 05e1abd: PostgreSQL 18.6 Compose service, loopback binding, named volume,
  private local connection configuration, EF/Npgsql context and probe migration,
  live/ready health separation, isolated Testcontainers EF write/read, and named
  volume durability. Owner corrected earlier password authentication failure.
- TECH-003 a77b651b655bc3301e8b77549b3c77ec179efe42: /api/v1 routing, OpenAPI
  contract test, Problem Details/unknown-route test, and root CI workflow.
- CI run https://github.com/Ravynnox/Calliqo/actions/runs/41551585014 passed both
  jobs for a77b651: frontend install/lint/one test/build; backend restore/Release
  build/four tests, zero failed/skipped and zero build warnings/errors.
- CI selected Node 26.13.0, npm 11.20.0, and SDK 10.0.204 under latestPatch.
- Issue #7 closure and Project Done were verified in the preceding task.
- Owner explicitly cancelled weather days-query validation; no 16-test target.
  Real request validation remains tracked under AUTH-002 (#10).

## Current OBS-001 implementation
- Owner added grafana/otel-lgtm:0.39.0 to Compose with optional observability
  profile, loopback ports 3000/4318, and lgtm_data mounted at /data.
- Owner output verifies Compose config validation, healthy container, and Grafana
  health 200/database ok (Grafana 13.3.0). Owner says browser/Explore step finished;
  this is interpreted as UI accessible, not verified data-source ingestion.
- API references OpenTelemetry.Extensions.Hosting, Instrumentation.AspNetCore,
  and Exporter.OpenTelemetryProtocol at 1.20.1, confirmed by source inspection.
- Owner reports expected build/four backend tests/clean audit after installation.
  Codex did not rerun these checks during the repository review.

## Current telemetry checkpoint
- HTTP telemetry helpers, Program registrations, Activity-based error trace IDs,
  native Npgsql subscription, SQL/data-source tag cleanup, SELECT 1 readiness,
  and both new tests are implemented. Source inspection confirms six Fact tests.
- Owner reports all instructed manual checks passed on 2026-11-04: database
  stopped => live 200/ready 503; restart => ready 200; matching error trace/log
  and error metrics; LGTM stopped => API live/ready remain 200 and responsive;
  LGTM restart => fresh correlated telemetry arrives without API restart.
- Owner reports privacy spot-checks, including driver failure events and HTTP
  query/user-agent markers, matched expectations. This is owner-reported evidence
  for tested paths; Codex did not inspect live payloads or rerun those checks.
- The processor removes SQL/data-source tags and status descriptions. It does
  not generally redact arbitrary exception-event content or future instrumentation.
- Prior whitespace issue is fixed: current git diff --check passes.
- Owner supplied final Release output: successful build; six tests passed,
  zero failed/skipped. Isolated Testcontainers databases started and cleaned up;
  request-log output confirms incoming trace ID propagation and readiness 200.
- All 26 intended checkpoint files are staged; staged whitespace checks pass.
  No unstaged changes existed before recording these final test results.
- Next: restage the updated evidence documents, owner commit/push, and verify
  both remote CI jobs for that exact OBS-001 commit.
- Keep #8 In Progress pending publication and CI. No source/configuration/Git
  changes or runtime verification were performed by Codex this turn.
## Repository-wide review result
Scanned 87 repository-owned files: read 86 text files, including 42 Markdown
files; one PNG was excluded from text review. Excluded .git internals, ignored
build/dependency output, actual .env, and external User Secrets.
- Completed first-computer setup, pins, baseline tests, persistence, health,
  versioned API, OpenAPI/error tests, and CI walkthroughs match source structure.
- At the audit checkpoint, HTTP telemetry wiring remained unapplied; owner has
  since implemented it, as recorded above. Other audit findings are historical.
- No duplicate server/OpenApiTests.cs, tests/PersistenceTests.cs, or misplaced
  workflow files were found. Root .github/workflows/ci.yml is the only ci.yml.
- Frontend manifest and lockfile root dependency declarations match; minimatch
  resolves to 11.0.1 in the lockfile. No broken relative Markdown links found.
- .env is ignored and untracked. Its contents were not read.
- Several historical docs incorrectly still described completed tasks as pending.
  Current summaries/banners were corrected; this was documentation drift, not
  proof the owner missed those steps. Older snippets are historical references.
- Review did not run builds, tests, Docker, audits, or live GitHub checks anew.

## Deliberately pending work
- FND-003 second-computer verification is explicitly deferred.
- AUTH/football/playbook/editor/game modules and additional frontend stack
  components are future roadmap work, not omitted current instructions.
- Identity, renderer, cloud provider/IaC tool, production telemetry stores,
  hosting, backups/restore, and required merge-check configuration remain open.
- PersistenceProbe is temporary: remove via migration before MVP once real
  module coverage provides equivalent persistence verification.
- Local PostgreSQL bootstrap role is privileged; production needs restricted
  runtime credentials and separate migration/provisioning permissions.
- PR-trigger CI and second-computer retrieval are not proven by a main-push run.

## Git and next action
- Branch main; last verified published commit a77b651. Earlier verification
  matched local HEAD, origin/main, and live GitHub main.
- Local OBS implementation changes include Compose, API package references,
  Program/readiness, new observability helpers/processor, and integration tests.
- Observability guides and completion/review documentation remain uncommitted;
  include reviewed docs with the next meaningful owner-created checkpoint.
- No staging/commit/push/branch or source/config changes were made by Codex.
- Present the complete actionable next step in chat. Documentation is reference
  and project memory; do not rely on the owner discovering instructions there.
- Owner confirms manual checks passed; final Release build and six tests are
  verified from supplied output. Restage evidence docs, commit/push, then CI.
- No source/configuration/Git changes or new local tests performed by Codex.

## Completion rule
Require verified acceptance and committed implementation/documentation before
finalizing a task. Do not claim planned telemetry exists. Cross-computer
continuity requires separate retrieval verification, not just a pushed commit.
