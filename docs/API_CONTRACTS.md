# API Contracts — TECH-003

> Review 2026-11-02: TECH-003 is complete in published a77b651 with successful CI. Error handling, /api/v1 routing, and OpenAPI test are implemented. Earlier checkpoint failure/pending notes below are historical. Weather validation is cancelled; real validation belongs to AUTH-002. OBS-001 telemetry wiring is a separate pending task.

## Status
Error responses and versioned routing/OpenAPI checkpoints passed per owner.
The owner confirmed tests pass after correcting both forecast caller URLs.
Weather-validation exercise is cancelled; API_VALIDATION.md records its disposition.
Validation demonstration is transferred to AUTH-002 (#10), on a real endpoint.
CI remains pending. TECH-002 is published in 05e1abd. Current endpoint paths:
/api/v1/weatherforecast, /health/live, /health/ready, and Development
/openapi/v1.json. No API source or test changes were applied by Codex.
## Proposed conventions
- Business REST endpoints use /api/v1. A literal route group is sufficient
  initially; add a versioning package only when requirements justify it.
- Health endpoints stay outside the business API prefix.
- Return explicit request/response DTOs rather than persistence entities.
- JSON uses camelCase; timestamps are UTC and IDs are opaque values.
- Success responses use meaningful HTTP status codes and DTOs without a
  mandatory universal envelope. Errors use Problem Details.
- Validation failures use 400 with field-specific errors; unknown resources
  use 404, authenticated permission failures 403, missing authentication 401,
  conflicting writes 409, and unexpected failures 500. Team visibility rules
  are refined and tested in AUTH tasks; conventions do not implement security.
- Paginated list contracts will define defaults, upper bounds, stable sorting,
  and filters when actual list endpoints are added. Do not invent unused APIs.
- Describe status codes, validation, and payloads in OpenAPI and test them.

## Error response contract
Use ASP.NET Core's built-in Problem Details services, plus a traceId extension
for correlating requests with logs. Errors use application/problem+json when
the client accepts JSON. Avoid returning exception messages, stack traces,
secrets, or SQL details. Expected error fields are type, title, status, and
traceId; detail/instance and validation errors are present when appropriate.
No blanket conversion of all exceptions into client-validation failures.

## Checkpoint 1 — owner edits to Program.cs
Destination: server/Calliqo.Api/Program.cs. Language: C#. Built into ASP.NET
Core; no additional NuGet dependency is needed.
After AddOpenApi and before builder.Build, add:

```csharp
builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Extensions["traceId"] =
            context.HttpContext.TraceIdentifier;
    };
});
```

Immediately after var app = builder.Build(), add:

```csharp
app.UseExceptionHandler();
app.UseStatusCodePages();
```

Exception handling has access to the registered Problem Details service.
Status-code pages fill otherwise empty error responses, including unmatched
routes. Existing health responses already have a body and remain unchanged.
The API routes are not being renamed during this first checkpoint.

## Create tests/Calliqo.IntegrationTests/ApiErrorTests.cs
Language: C#. Uses the existing xUnit/Mvc.Testing dependencies. This test
verifies the HTTP error contract for an unknown route, not framework internals.

```csharp
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Calliqo.IntegrationTests;

public sealed class ApiErrorTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ApiErrorTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task UnknownRoute_ReturnsProblemDetailsWithTraceId()
    {
        using var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost"),
            AllowAutoRedirect = false
        });
        client.DefaultRequestHeaders.Accept.ParseAdd("application/json");

        using var response = await client.GetAsync("/api/v1/does-not-exist");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal(404, document.RootElement.GetProperty("status").GetInt32());
        Assert.False(string.IsNullOrWhiteSpace(
            document.RootElement.GetProperty("traceId").GetString()));
    }
}
```

## Verify checkpoint 1
With Docker Desktop running for the existing persistence test, run from
repository root:

```powershell
dotnet build Calliqo.slnx
dotnet test Calliqo.slnx
```

Expected: warning-free build and three passing backend tests. No packages
changed, so another audit is not needed solely for these source edits.
If JSON is missing, check services and middleware placement and saved files.
The unknown-route test does not prove a real versioned business endpoint
exists, nor does it exercise an unexpected 500 failure. Record that limit.
Do not add a public throw endpoint just to exercise exception handling.

## Remaining TECH-003 checkpoints
1. Move the starter contract into /api/v1 and document OpenAPI output.
2. Weather-validation exercise cancelled on 2026-11-02; demonstrate request
   validation on the first real sign-in/team endpoint under AUTH-002 (#10).
3. Add CI through owner-created .github/workflows configuration: frontend
   npm ci/test/build/lint and .NET restore/build/test using pinned tool inputs.
   Linux runner must support Docker for isolated Testcontainers tests.
4. Verify CI run results and API documentation. CI needs no development
   .env, User Secrets, or production credentials for the isolated tests.
5. Owner reviews/commits/pushes code and docs with #7; close only when all
   acceptance criteria and remote checks are verified.

Current status: both foundation checkpoints passed per owner. Weather-validation
work is cancelled; real validation is tracked under AUTH-002. CI is pending.
No workflow, API/test source, packages, Git commits, or branches were changed by Codex.

Reference: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-10.0

## Checkpoint 2 — versioned route and OpenAPI contract
Status: owner implemented this checkpoint. Supplied 2026-11-02 output shows
a successful build and four tests: three passed, one failed. UnitTest1.cs
requests /api/v1/weatherforecas (missing final t), producing 404; the HTTP
sample has the same typo. Correct both to /api/v1/weatherforecast and rerun
the tests. The OpenAPI contract test passed. No new package
is needed. Introduce one literal version prefix, sufficient for the initial
API; simultaneous version negotiation/provider selection is not required yet.

### Program.cs
Immediately before the existing weatherforecast MapGet in
server/Calliqo.Api/Program.cs, insert this complete C# declaration:

```csharp
var api = app.MapGroup("/api/v1")
    .WithGroupName("v1");
```

Change that handler's receiver from app.MapGet to api.MapGet, retaining its
relative /weatherforecast path and body. Change its WithName value from
GetWeatherForecast to GetWeatherForecastV1. Keep operational health mappings
on app, outside this business API group. WithGroupName associates the route
with the existing default v1 OpenAPI document.

### Update existing callers
In tests/Calliqo.IntegrationTests/UnitTest1.cs, change the forecast GetAsync
path to /api/v1/weatherforecast. In server/Calliqo.Api/Calliqo.Api.http,
update the sample GET path to /api/v1/weatherforecast as well.
The browser sample URL becomes http://localhost:5033/api/v1/weatherforecast.
The development document remains /openapi/v1.json.

### Create tests/Calliqo.IntegrationTests/OpenApiTests.cs
Language: C#. Uses existing xUnit, Mvc.Testing, and framework JSON APIs.
The test explicitly chooses Development because OpenAPI is exposed only there.

```csharp
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Calliqo.IntegrationTests;

public sealed class OpenApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public OpenApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Document_DescribesVersionedForecastEndpoint()
    {
        using var application = _factory.WithWebHostBuilder(builder =>
            builder.UseEnvironment("Development"));
        using var client = application.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost"),
            AllowAutoRedirect = false
        });

        using var response = await client.GetAsync("/openapi/v1.json");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var paths = document.RootElement.GetProperty("paths");
        Assert.True(paths.TryGetProperty("/api/v1/weatherforecast", out var endpoint));
        var operation = endpoint.GetProperty("get");
        Assert.Equal("GetWeatherForecastV1", operation.GetProperty("operationId").GetString());
        Assert.False(paths.TryGetProperty("/weatherforecast", out _));
    }
}
```

### Verify from repository root
With Docker Desktop running for the existing persistence test:

```powershell
dotnet build Calliqo.slnx
dotnet test Calliqo.slnx
```

Expected: warning-free build and four passing backend tests. Forecast tests
now verify the versioned endpoint; the new test verifies it is advertised in
OpenAPI with the expected operation ID and without the old unversioned path.
Request-validation demonstration is deferred to AUTH-002 (#10) on a real endpoint.
No application, test, HTTP example, or configuration file has been changed by Codex.

References:
- https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/route-handlers?view=aspnetcore-10.0
- https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/include-metadata?view=aspnetcore-10.0

## Latest checkpoint update — 2026-11-02
Owner reports all four existing tests pass after caller URL corrections. Earlier
failure notes are historical. The weather-validation exercise is cancelled;
[disposition](API_VALIDATION.md) replaces its walkthrough. Real validation and
contract tests are tracked under AUTH-002 (#10). TECH-003 remains open for CI
and publication. Existing weather coverage stays until real endpoints replace it.
