# API telemetry walkthrough — OBS-001 checkpoint 2B

> Update 2026-11-04: owner implemented this HTTP telemetry code and the fifth test,
> confirmed by source inspection, and indicates ready to continue. Database
> tracing is next; no independent runtime rerun by Codex. Earlier prepared-only
> status notes describe the previous checkpoint.

## Status, objective, and prerequisites
Prepared for owner implementation on 2026-11-02. Three OpenTelemetry references
at 1.20.1 are present in the API project, confirmed by source inspection. Owner
reports expected successful build, four passing tests, and clean vulnerability
report after installation. Codex did not rerun those checks.

Objective: export incoming HTTP traces, server duration metrics, and correlated
request-summary logs to local LGTM, while keeping export opt-in for tests/CI.
Prerequisites: Grafana is accessible, LGTM is running on loopback ports 3000/4318,
and Docker is running for the existing database test. No additional packages.
Database spans, controlled 503 checks, exporter-outage checks, and broader
privacy verification follow this checkpoint. Do not close OBS-001 yet.

## 1. Create server/Calliqo.Api/Infrastructure/Observability/ObservabilityExtensions.cs
Purpose: centralize telemetry registration and request-summary middleware.
Language: C#. This is a NEW source file for the owner to create.

```csharp
using System.Diagnostics;
using Microsoft.AspNetCore.Routing;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Calliqo.Api.Infrastructure.Observability;

public static class ObservabilityExtensions
{
    private const string RequestLogCategory = "Calliqo.Requests";

    public static WebApplicationBuilder AddCalliqoObservability(
        this WebApplicationBuilder builder)
    {
        builder.Logging.AddJsonConsole(options =>
        {
            options.IncludeScopes = false;
            options.UseUtcTimestamp = true;
            options.TimestampFormat = "yyyy-MM-dd'T'HH:mm:ss.fff'Z'";
        });

        if (!builder.Configuration.GetValue<bool>("Observability:Enabled"))
        {
            return builder;
        }

        var endpointText = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];
        if (!Uri.TryCreate(endpointText, UriKind.Absolute, out var endpoint) ||
            endpoint.Scheme is not ("http" or "https"))
        {
            throw new InvalidOperationException(
                "Observability requires an absolute HTTP(S) OTLP endpoint.");
        }

        builder.Logging.AddFilter<OpenTelemetryLoggerProvider>((category, level) =>
            category == RequestLogCategory && level >= LogLevel.Information);

        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource
                .AddService("Calliqo.Api")
                .AddAttributes(new Dictionary<string, object>
                {
                    ["deployment.environment.name"] = builder.Environment.EnvironmentName
                }))
            .WithTracing(tracing => tracing.AddAspNetCoreInstrumentation(options =>
            {
                options.RecordException = false;
                options.EnrichWithHttpResponse = (activity, response) =>
                {
                    var route = GetRoute(response.HttpContext);
                    activity.DisplayName = $"{response.HttpContext.Request.Method} {route}";
                    activity.SetTag("url.path", null);
                    activity.SetTag("url.query", null);
                    activity.SetTag("user_agent.original", null);
                };
            }))
            .WithMetrics(metrics => metrics.AddAspNetCoreInstrumentation())
            .WithLogging(configureBuilder: null, configureOptions: options =>
            {
                options.IncludeFormattedMessage = true;
                options.IncludeScopes = false;
            })
            .UseOtlpExporter(OtlpExportProtocol.HttpProtobuf, endpoint);

        return builder;
    }

    public static WebApplication UseCalliqoRequestLogging(this WebApplication app)
    {
        var logger = app.Services.GetRequiredService<ILoggerFactory>()
            .CreateLogger(RequestLogCategory);

        app.Use(async (HttpContext context, RequestDelegate next) =>
        {
            var started = Stopwatch.GetTimestamp();
            var statusCode = StatusCodes.Status500InternalServerError;
            try
            {
                await next(context);
                statusCode = context.Response.StatusCode;
            }
            finally
            {
                var elapsedMs = Stopwatch.GetElapsedTime(started).TotalMilliseconds;
                var traceId = Activity.Current?.TraceId.ToString() ?? context.TraceIdentifier;
                logger.Log(
                    statusCode >= 500 ? LogLevel.Error : LogLevel.Information,
                    new EventId(1000, "RequestCompleted"),
                    "HTTP {Method} {Route} responded {StatusCode} in {ElapsedMs} ms TraceId={TraceId}",
                    context.Request.Method, GetRoute(context), statusCode, elapsedMs, traceId);
            }
        });

        return app;
    }

    private static string GetRoute(HttpContext context) =>
        (context.GetEndpoint() as RouteEndpoint)?.RoutePattern.RawText ?? "unmatched";
}
```

Why this shape:
- The host owns provider lifetime. ConfigureResource gives all three signals the
  same service/environment identity. UseOtlpExporter registers all three exporters
  and appends /v1/logs, /v1/metrics, and /v1/traces to the base HTTP endpoint.
- Do not also call AddOtlpExporter separately; that conflicts with this registration.
- Registration is opt-in, independent of Development mode. The CI workflow does
  not set Observability:Enabled or require the optional LGTM Compose profile.
- Console logs use JSON. Exported logs are deliberately limited to the safe
  request-summary category. Framework/EF logs remain in their existing local
  logging configuration but are not sent through this OTLP provider filter.
- We log the route TEMPLATE (or unmatched), status, method, duration, and trace ID.
  Never substitute Request.Path, query strings, bodies, headers, identities,
  exception messages, or connection strings into this request summary.
- ASP.NET Core instrumentation supplies raw URL/user-agent attributes by default.
  The response enrichment callback removes those fields after instrumentation
  populates them and uses a route-based span name. Exception-event capture stays
  off. Other SDK/resource attributes still need manual review; this is not a
  general-purpose redaction mechanism for arbitrary future instrumentation.
- The request logger wraps the exception/status handlers, so handled failures
  have their final HTTP status when logged. A pipeline exception that escapes
  is logged as 500; aborted-client handling will need refinement when applicable.
- Trace/span context is attached automatically to exported ILogger records while
  the request Activity is active. TraceId is also a structured log field for
  console inspection. It belongs in logs, not metric labels.

## 2. Edit server/Calliqo.Api/Program.cs
Purpose: enable the helpers and align error trace IDs with W3C trace correlation.
Add this C# import alongside the existing imports:

```csharp
using System.Diagnostics;
```

The existing Calliqo.Api.Infrastructure.Observability import already makes the
extension methods available. Immediately AFTER builder creation, add:

```csharp
builder.AddCalliqoObservability();
```

In the existing CustomizeProblemDetails callback replace only the traceId value:

```csharp
context.ProblemDetails.Extensions["traceId"] =
    Activity.Current?.TraceId.ToString() ?? context.HttpContext.TraceIdentifier;
```

Immediately AFTER app creation and BEFORE UseExceptionHandler, add:

```csharp
app.UseCalliqoRequestLogging();
```

Keep all existing services, versioned routes, health endpoints, exception
handling, and status-code pages. Do not add a diagnostic/throw endpoint.

## 3. Extend tests/Calliqo.IntegrationTests/ApiErrorTests.cs
Purpose: verify that a client's W3C trace context is reflected in the error
response, making the returned identifier useful for trace lookup.
Inside the existing class, add this C# test beside the existing test:

```csharp
[Fact]
public async Task UnknownRoute_ReturnsIncomingTraceId()
{
    using var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
    {
        BaseAddress = new Uri("https://localhost"),
        AllowAutoRedirect = false
    });
    const string traceId = "11111111111111111111111111111111";
    client.DefaultRequestHeaders.Add(
        "traceparent", $"00-{traceId}-2222222222222222-01");
    client.DefaultRequestHeaders.Accept.ParseAdd("application/json");

    using var response = await client.GetAsync("/api/v1/does-not-exist");
    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
    Assert.Equal(traceId, document.RootElement.GetProperty("traceId").GetString());
}
```

This checks the HTTP correlation contract. It does not prove an exporter or
collector received anything. Manual ingestion checks below cover that separately.

## 4. Build and test from repository root
Use a terminal WITHOUT the telemetry environment variables from the next section.
Keep Docker Desktop running for PersistenceTests. PowerShell commands:

```powershell
dotnet build Calliqo.slnx
dotnet test Calliqo.slnx
```

Expected: successful build and FIVE tests passing. No additional packages changed
in checkpoint 2B, so the clean audit just reported need not be repeated solely
for these source edits. Share any compile/test failure before proceeding.

## 5. Run the API with export enabled
In a separate PowerShell terminal at repository root, set process-local variables
and start the HTTP profile. These values are non-secret local configuration:

```powershell
$env:Observability__Enabled = 'true'
$env:OTEL_EXPORTER_OTLP_ENDPOINT = 'http://127.0.0.1:4318'
$env:OTEL_METRIC_EXPORT_INTERVAL = '5000'
dotnet run --project server/Calliqo.Api --launch-profile http
```

Five-second metric export is for this local verification; the SDK's default
interval is longer. Variables affect this terminal/child processes, not Git,
User Secrets, .env, other terminals, or GitHub CI. Do not set them globally.
When finished, stop the API with Ctrl+C and close this terminal, or remove only
these three environment variables before running tests from it.

## 6. Generate and find telemetry
From another terminal, send these requests to the existing API. The fixed trace
ID below is a disposable diagnostic value; it is not a credential. Do not reuse
it for ongoing traffic after this verification.

```powershell
curl.exe -i -H "traceparent: 00-11111111111111111111111111111111-2222222222222222-01" -H "Accept: application/json" http://localhost:5033/api/v1/does-not-exist
curl.exe -i http://localhost:5033/health/live
```

Expected: first 404 Problem Details with traceId 11111111111111111111111111111111;
second 200 Healthy. The request-summary console JSON should contain Method,
Route, StatusCode, ElapsedMs, and TraceId. The unmatched request must show the
route as unmatched, without copying the raw URL into the summary.

In Grafana Explore, select an appropriate recent time range (such as last
15 minutes) and refresh after several seconds:
- Tempo: look up trace ID 11111111111111111111111111111111. Inspect the server
  span resource service.name=Calliqo.Api, route/unmatched name, and HTTP status.
- Loki: select service_name=Calliqo.Api using the label browser. Inspect a
  request-summary entry and its trace_id metadata. Search the fixed ID from
  the message if needed; it should match the Tempo trace and Problem Details.
- Metrics data source: use the metric browser to find the HTTP server request
  duration histogram (OTel name http.server.request.duration, often translated
  to http_server_request_duration_seconds_*). Verify its count/buckets increase
  after requests. Inspect available labels instead of assuming every resource
  attribute is a metric label. Do not expect per-request IDs in metrics.

A 404 is a client error response, not proof of a server-error trace status.
Controlled 503 verification follows with the existing database readiness path.
Privacy spot-check: span attributes must omit url.path, url.query, and
user_agent.original. No request bodies/auth headers or raw query values should
appear in the exported summary logs. Inspect resource attributes as well.

## Acceptance and handoff
Share build/test results and whether the SAME trace ID appears in the error,
request log, and Tempo trace, plus whether the duration metric is present.
Failures to find data require checking time range, service labels, API restart,
opt-in variables, collector startup, and protocol/port. Do not claim telemetry
works merely because the application starts or a data source exists.

Keep OBS-001 open. Database spans, server-error/outage behavior, privacy checks,
CI compatibility, and source/docs publication remain outstanding. Suggested
owner checkpoint reference: OBS-001: add correlated HTTP telemetry (#8).
No source/configuration/Git edits or local tests were performed by Codex.

## Primary references
- [OTLP all-signal export and base-URL behavior](https://github.com/open-telemetry/opentelemetry-dotnet/blob/core-1.20.1/src/OpenTelemetry.Exporter.OpenTelemetryProtocol/README.md)
- [ASP.NET Core attributes and enrichment ordering](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/blob/Instrumentation.AspNetCore-1.20.1/src/OpenTelemetry.Instrumentation.AspNetCore/README.md)
- [OpenTelemetry .NET logging](https://opentelemetry.io/docs/languages/dotnet/logs/getting-started-aspnetcore/)

## Database-tracing checkpoint — proposed, 2026-11-04
Complete actionable instructions are delivered directly in chat. Planned edits:
subscribe to the existing Npgsql ActivitySource; add NpgsqlTracePrivacyProcessor
to remove SQL text/data-source tags and error status descriptions; change the
existing readiness check to execute SELECT 1; add an isolated readiness HTTP
test to PersistenceTests. No new package, migration, or public endpoint needed.
Expected suite after owner implementation: six tests. Verify a readiness HTTP
span with a PostgreSQL child and matching log trace ID in Grafana. Native driver
exception-event review remains part of later failure/privacy verification; the
tag processor is not a complete exception-event redactor. No DB tracing changes
are implemented or tested by Codex. OBS-001 remains open.

## Database source review and next checks — 2026-11-04
Owner implemented the Npgsql source subscription, tag-cleanup processor,
SELECT 1 readiness, and isolated readiness test. Six backend Fact tests are
present. Owner indicates ready to continue; Codex has not executed those tests
or independently inspected live ingestion. Prior proposed-only DB notes above
are historical. Whitespace check found a spaces-only line at PersistenceTests.cs:83.

The following manual validation is prepared and delivered fully in chat:
- Database stopped: /health/live remains 200; /health/ready returns 503; HTTP
  error trace and request-summary log correlate; inspect driver exception events.
- Database restarted: ready returns 200 with a PostgreSQL child span again.
- LGTM stopped while PostgreSQL/API remain running: live/ready remain 200 and
  requests complete promptly. Export is asynchronous; outage-period delivery
  is not guaranteed. This check does not establish a durable telemetry queue.
- LGTM restarted: verify health, then send a fresh traced request and confirm
  fresh logs/traces/metrics arrive without restarting the API.
- Privacy marker request on /health/live: verify raw query/user-agent marker
  values do not appear in exported request span/log fields or metric labels.

None of those outage checks has been performed by Codex. Use stop/start for
individual services; no volume deletion, data reset, or code edits are needed.
Keep OBS-001 open until actual results, privacy review, tests, CI, and publication
are recorded. No new code changes are proposed solely for this manual checkpoint.

## Manual verification reported passed — 2026-11-04
Owner states every instructed check matched expectations: database outage and
recovery, HTTP/database/log correlation, error metrics, LGTM outage leaving the
API available, exporter recovery without API restart, and tested telemetry
privacy markers/driver events. Codex did not execute or independently observe
these manual checks. Whitespace issue at PersistenceTests.cs:83 is resolved.
Six tests are present by inspection. Final Release checks, owner publication,
and CI remain before issue closure. Historical pending-manual notes above are
superseded by this reported result; no general exception-event redactor is claimed.
