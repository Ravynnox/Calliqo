# Observability setup — OBS-001 (#8)

> Review 2026-11-02: local LGTM startup/browser access and the three package installations are completed per inspection/owner results. API telemetry wiring in OBSERVABILITY_API.md is still pending. Do not reinstall or recreate completed steps merely to catch up.

## Status and objective
Owner approved starting OBS-001 on 2026-11-02 after TECH-003 completed in
published a77b651 and successful CI run 41551585014. This is owner-applied
mentorship guidance; no source/configuration/container changes were made by Codex.
Checkpoint 1 startup is verified from owner output: Compose validation passed,
LGTM 0.39.0 is healthy, and Grafana /api/health returned 200/database ok.
Owner reports the browser/Explore step finished; interpreted as accessible.
No individual data-source connectivity test or API ingestion is verified yet.
No API telemetry is exported yet.

## Plan and prerequisites
1. Start and verify a local Grafana LGTM development stack (current checkpoint).
2. Add version-reviewed OpenTelemetry .NET packages and API configuration for
   request traces, HTTP/server metrics, and correlated structured logs via OTLP.
3. Instrument Npgsql, verify a real database operation under its request trace,
   and align Problem Details traceId with distributed trace correlation.
4. Verify latency, errors, correlation, exporter-unavailable behavior, and
   telemetry privacy; run relevant tests/build/audits and existing CI.
5. Owner publishes implementation/docs and verified evidence before closing #8.

Prerequisites: Docker Desktop Linux engine works; existing ignored .env satisfies
Compose's PostgreSQL password requirement; ports 3000 and 4318 are available.
Do not paste .env, User Secrets, or resolved Compose configuration into chat.
No Grafana Cloud account or cloud-provider decision is required here.

## Concepts and scope
OpenTelemetry produces telemetry. OTLP transports it to a collector. Grafana
queries the backend stores to display logs, metrics, and traces. The LGTM image
bundles those development components, avoiding separate service wiring for the
first local verification. It is intended for development/demo/testing, not the
future production environment. Selecting it here does not select cloud storage,
managed hosting, production access controls, or infrastructure-as-code tooling.
Those decisions remain CLOUD-001/INFRA-001; dashboards/alerts remain OBS-002.

We will correlate individual logs/traces by trace ID. Metrics aggregate requests;
correlate them by service, route, and time, with exemplars where supported. Do not
put trace IDs, player IDs, raw URLs, or other unbounded values in metric labels.
No request bodies, cookies, auth headers, secrets, or SQL parameter values belong
in telemetry. Review default HTTP/database tags before exports are enabled.

## Checkpoint 1: edit repository-root compose.yaml
Purpose: add optional local telemetry services without changing PostgreSQL.
Language: YAML. Under the existing services mapping, after the postgres service
and BEFORE the root-level volumes mapping, insert this service block:

```yaml
  lgtm:
    image: grafana/otel-lgtm:0.39.0
    profiles: [observability]
    ports:
      - "127.0.0.1:3000:3000"
      - "127.0.0.1:4318:4318"
    volumes:
      - lgtm_data:/data
```

In the EXISTING root volumes mapping, add lgtm_data beside postgres_data so its
complete declaration becomes the following YAML. Do not create two volumes keys:

```yaml
volumes:
  postgres_data:
  lgtm_data:
```

The image version comes from Grafana's v0.39.0 release, verified on 2026-11-02.
An explicit version tag is used instead of latest; tags are not immutable digest
pins. Record the pulled image digest with the check below for evidence.
Profile observability makes the extra stack opt-in. Loopback bindings keep the
browser and OTLP interfaces local. Port 4318 is OTLP HTTP/protobuf; configure the
API to use that protocol in checkpoint 2. Do not send telemetry using gRPC to this
HTTP port. No extra database port or gRPC port is required for this plan.
/data is the image's documented data path. The named volume retains local
telemetry across container recreation; it is not production retention/backup.

## Start and verify from repository root
PowerShell commands, purpose: validate YAML without printing resolved secrets,
start only the LGTM service, inspect startup, and query Grafana health:

```powershell
docker compose --profile observability config --quiet
docker compose --profile observability up -d lgtm
docker compose --profile observability ps lgtm
docker compose --profile observability logs --tail 80 lgtm
curl.exe -i http://127.0.0.1:3000/api/health
docker image inspect grafana/otel-lgtm:0.39.0 --format '{{json .RepoDigests}}'
```

First startup downloads an image and starts multiple components; it can take a
few minutes. A running container alone does not prove readiness. If curl cannot
connect immediately, wait for startup progress in logs, then retry. If a port is
occupied, report the conflict rather than changing mappings independently.
Do not run docker compose down -v; it would also remove database storage.

Open http://127.0.0.1:3000. The upstream documented local default login is
admin / admin; follow any first-login password prompt locally. These are image
defaults, not application credentials; do not reuse them for hosted environments.
Open Explore and check that telemetry data sources are provisioned. They may
be empty: Calliqo has not been instrumented yet. No request telemetry is claimed.

## Acceptance and result handoff
Checkpoint 1 passes when Compose validation succeeds, the service stays running,
Grafana /api/health returns 200 with a healthy database, and the browser UI/data
sources are accessible. Logs should show the collector/backend components have
started; Grafana's health endpoint alone does not prove OTLP ingestion or the
other stores are ready. Actual ingestion is verified after API instrumentation.

Share Compose status, HTTP health response, and whether Explore is available.
Record the pulled digest. No new automated tests are needed for this manual
startup checkpoint. API builds/tests need not be repeated solely for this service
addition. Keep OBS-001 open until all telemetry acceptance criteria are verified.

## Documentation and Git
Codex maintains this guide, state/roadmap, and task snapshot. Owner edits Compose
and runs Docker; no implementation, Docker startup, or Git mutation is performed
by Codex. Existing TECH-003 post-run completion notes remain local and belong in
the next meaningful owner-reviewed checkpoint. Suggested eventual commit title:
OBS-001: establish local telemetry collection (#8). Reference #8 without closing
keywords until all criteria pass and source/docs are published. CI should remain
independent of the optional developer observability profile.

## References
- [Grafana LGTM development backend](https://grafana.com/docs/opentelemetry/docker-lgtm/)
- [Grafana LGTM v0.39.0 release](https://github.com/grafana/docker-otel-lgtm/releases/tag/v0.39.0)
- [LGTM ports, persistence, and local login](https://github.com/grafana/docker-otel-lgtm)
- [OpenTelemetry .NET](https://opentelemetry.io/docs/languages/dotnet/getting-started/)

## Startup evidence — 2026-11-02
Owner supplied successful Compose validation, image pull, named-volume creation,
and container startup. calliqo-lgtm-1 is healthy with loopback bindings on
3000 and 4318. Grafana health returned 200, database ok, version 13.3.0.
Direct Compose inspection confirms the intended profile, ports, image, and
/data volume. Browser Explore/data-source visibility and image digest remain
unreported; API ingestion/correlation are not verified. No Docker commands or
configuration changes were performed by Codex.

## Checkpoint 2A — API dependency installation
Status: walkthrough prepared, not installed/tested. Owner reported completion
of the Grafana browser step. This is interpreted as successful UI access;
no individual data-source connectivity or API ingestion is claimed.

Objective: add the minimum packages for host-managed OpenTelemetry, incoming
request instrumentation, and OTLP export. Versions 1.20.1 were verified on the
official NuGet package pages on 2026-11-02; they support .NET 10 via compatible
target frameworks. Runtime integration still requires restore/build/test checks.

| Package | Purpose |
|---|---|
| OpenTelemetry.Extensions.Hosting | Integrate telemetry provider startup/shutdown with the application host |
| OpenTelemetry.Instrumentation.AspNetCore | Collect incoming HTTP request traces and server metrics |
| OpenTelemetry.Exporter.OpenTelemetryProtocol | Export logs, metrics, and traces through OTLP |

Destination: server/Calliqo.Api/Calliqo.Api.csproj. Run these PowerShell commands
from the repository root; each modifies that project's package references:

```powershell
dotnet add server/Calliqo.Api/Calliqo.Api.csproj package OpenTelemetry.Extensions.Hosting --version 1.20.1
dotnet add server/Calliqo.Api/Calliqo.Api.csproj package OpenTelemetry.Instrumentation.AspNetCore --version 1.20.1
dotnet add server/Calliqo.Api/Calliqo.Api.csproj package OpenTelemetry.Exporter.OpenTelemetryProtocol --version 1.20.1
```

Prerequisites: existing baseline passes, network access to NuGet, and Docker
Desktop running for the persistence test. These references alone do not enable
collection/export. Program.cs, endpoint configuration, privacy controls, and
correlation changes follow after dependency compatibility is checked.

Validation from repository root, purpose: verify the changed dependency graph,
existing application behavior, and current vulnerability reports:

```powershell
dotnet build Calliqo.slnx
dotnet test Calliqo.slnx
dotnet list Calliqo.slnx package --include-transitive --vulnerable
```

Expected: successful build, four existing tests passing, and no reported known
vulnerable packages from configured sources. Report any restore/version conflict
or audit finding before proceeding. No new tests are needed just to reference
packages; existing tests verify the dependency change does not break the baseline.

Share build/test/audit results. Codex will then inspect references and prepare
API logs/metrics/traces configuration with OTLP HTTP/protobuf on localhost:4318.
Do not infer telemetry works merely because installation succeeds. No package
or API changes were performed by Codex. Keep #8 open; include dependency changes
and current documentation in the eventual owner-reviewed OBS-001 checkpoint.

Package references:
- https://www.nuget.org/packages/OpenTelemetry.Extensions.Hosting/1.20.1
- https://www.nuget.org/packages/OpenTelemetry.Instrumentation.AspNetCore/1.20.1
- https://www.nuget.org/packages/OpenTelemetry.Exporter.OpenTelemetryProtocol/1.20.1

## Checkpoint 2A result and next walkthrough
Owner reports expected build/test/audit results: successful build, four tests
passing, and no reported vulnerabilities. Direct source inspection confirms
the three OpenTelemetry 1.20.1 references. Codex did not run those checks.
Next: [API telemetry configuration](OBSERVABILITY_API.md), prepared for owner
implementation and not yet applied or verified.
