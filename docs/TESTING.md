# Testing

## Current status
TECH-001 (#5): owner installed the frontend harness and added the test/config.
First run failed with Invalid Chai property: toBeInTheDocument because
src/test/setup.ts was empty. Owner saved the setup contents and subsequently
confirmed tests pass; direct inspection confirms matcher registration and
cleanup are present. Owner subsequently confirmed build/lint/audit after
adding test packages pass. No test
dependencies, configuration, or test source files have been created by Codex.
Frontend build/lint,
browser interaction, backend build/endpoints, and current dependency audits
have passed as recorded in DEVELOPMENT_STATE.md. Owner supplied backend
results: one xUnit integration test passed, solution build passed, and the
NuGet audit reports no vulnerable packages for API or test project.
Database and end-to-end suites come with their workflows.

## Frontend baseline — owner walkthrough
Objective: establish Vitest, DOM rendering, and user interaction assertions
using the existing starter counter. This temporary scaffold test verifies
the harness; replace it when application behavior replaces the starter.
Prerequisites: Node 26.13.0, npm 11.20.0, existing working React/Vite scaffold.

### Install dependencies and add scripts
Run from client/. These commands modify client/package.json and its lockfile.
Versions were checked against registry peer/engine metadata on 2026-11-01:
Vitest supports Vite 8/Node 26; React Testing Library supports React 19.
The explicit DOM dependency satisfies the React Testing Library peer.

```powershell
npm install --save-dev --save-exact vitest@5.0.5 jsdom@30.1.3 @testing-library/react@16.3.3 @testing-library/dom@10.4.2 @testing-library/jest-dom@7.0.1 @testing-library/user-event@14.6.8
npm pkg set 'scripts.test=vitest run' 'scripts.test:watch=vitest'
```

Vitest is the runner. jsdom supplies browser-like DOM APIs rather than a real
browser. React Testing Library renders/query components, user-event models
interactions, and jest-dom adds readable assertions. Versions are pinned at
installation; new dependency audits and actual test results remain necessary.

### Replace client/vite.config.ts
Language: TypeScript. Uses the installed Vitest and React plugin. Vitest's
defineConfig extends Vite configuration with typed test settings.

```typescript
import react from '@vitejs/plugin-react'
import { defineConfig } from 'vitest/config'

export default defineConfig({
  plugins: [react()],
  test: {
    environment: 'jsdom',
    setupFiles: ['./src/test/setup.ts'],
  },
})
```

### Create client/src/test/setup.ts
Language: TypeScript. Registers DOM matchers and cleans rendered components
after each test. Uses explicit Vitest imports instead of test globals.

```typescript
import '@testing-library/jest-dom/vitest'
import { cleanup } from '@testing-library/react'
import { afterEach } from 'vitest'

afterEach(() => {
  cleanup()
})
```

### Create client/src/App.test.tsx
Language: TypeScript/TSX. Depends on the existing App component and installed
test libraries. Queries the button by accessible role/name, clicks it, and
checks the visible outcome without inspecting component internals.

```tsx
import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { describe, expect, it } from 'vitest'
import App from './App'

describe('App', () => {
  it('increments the displayed count when the button is clicked', async () => {
    const user = userEvent.setup()
    render(<App />)

    const button = screen.getByRole('button', { name: /count is 0/i })
    await user.click(button)

    expect(
      screen.getByRole('button', { name: /count is 1/i }),
    ).toBeInTheDocument()
  })
})
```

### Verify
Run from client/ after edits:

```powershell
npm run test
npm run build
npm run lint
npm audit
```

Expected: one discovered passing test, successful production build/lint,
and no unreviewed dependency findings. npm run test exits after one run;
npm run test:watch reruns affected tests until stopped with Ctrl+C.
If a test cannot find the initial button, inspect the accessible name in App.
If document is undefined, verify the jsdom test environment. If test matchers
are missing, verify setupFiles and the jest-dom/vitest import.
Report package conflicts or newly introduced audit findings before force-fixing.

Acceptance: the user interaction test passes and existing build/lint remain
healthy. This does not verify physical browser layout, touch, team isolation,
or API behavior. Those need appropriate future tests. No coverage threshold
or placeholder assertions are introduced for this baseline.

## Git and handoff
Keep test code with its configuration and lockfile changes when the owner
reviews the TECH-001 checkpoint. Reference issue #5 in commits; do not close
it until backend baseline testing and all acceptance criteria are complete.
Codex updates this document with verified results; the owner stages, commits,
and pushes. Second-computer verification remains pending under FND-003.

## Official references
- https://vitest.dev/guide/
- https://vitest.dev/guide/environment.html
- https://testing-library.com/docs/react-testing-library/setup/

## Backend baseline — owner walkthrough
Status: implemented by the owner. One backend integration test passed;
solution build and transitive NuGet audit passed per owner-provided output.
The class is WeatherForecastTests but still resides in UnitTest1.cs; rename
the file to WeatherForecastTests.cs before the checkpoint for consistency.
Use xUnit and WebApplicationFactory to test the real request pipeline and
JSON serialization in memory. No running API process, database, Docker, or
browser is required. Replace this temporary forecast test when the starter
endpoint is replaced by application behavior.

Run from the repository root. These commands modify the test project and solution:

```powershell
dotnet new xunit --name Calliqo.IntegrationTests --output tests/Calliqo.IntegrationTests --framework net10.0
dotnet add tests/Calliqo.IntegrationTests/Calliqo.IntegrationTests.csproj reference server/Calliqo.Api/Calliqo.Api.csproj
dotnet add tests/Calliqo.IntegrationTests/Calliqo.IntegrationTests.csproj package Microsoft.AspNetCore.Mvc.Testing --version 10.0.13
dotnet sln Calliqo.slnx add tests/Calliqo.IntegrationTests/Calliqo.IntegrationTests.csproj
```

The template supplies xUnit runner packages. Mvc.Testing supplies the
in-memory host; 10.0.13 is a stable .NET 10 servicing version. Audit the
resulting dependencies rather than assuming they are clean.

Append this C# declaration at the very end of server/Calliqo.Api/Program.cs,
after the WeatherForecast record (do not replace the file):

```csharp
public partial class Program { }
```

This exposes the generated entry-point type to the test project without
changing endpoint behavior.

Rename tests/Calliqo.IntegrationTests/UnitTest1.cs to WeatherForecastTests.cs
and replace its complete C# contents with:

```csharp
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Calliqo.IntegrationTests;

public class WeatherForecastTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public WeatherForecastTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetWeatherForecast_ReturnsSuccessfulJsonArray()
    {
        using var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost"),
            AllowAutoRedirect = false
        });
        using var response = await client.GetAsync("/weatherforecast");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal(JsonValueKind.Array, document.RootElement.ValueKind);
        Assert.NotEmpty(document.RootElement.EnumerateArray());

        foreach (var forecast in document.RootElement.EnumerateArray())
        {
            Assert.Equal(JsonValueKind.String, forecast.GetProperty("date").ValueKind);
            Assert.Equal(JsonValueKind.Number, forecast.GetProperty("temperatureC").ValueKind);
        }
    }
}
```

HTTPS is a logical in-memory request scheme here, avoiding redirect warnings;
it creates no real TLS listener and checks no certificate. The test checks
status, media type, JSON shape, and basic field types without asserting
random temperatures or dates. Remove the template's empty placeholder test.

Verify from repository root:

```powershell
dotnet test Calliqo.slnx
dotnet build Calliqo.slnx
dotnet list Calliqo.slnx package --include-transitive --vulnerable
```

Expected: one discovered passing backend test, successful build, and no
unreviewed audit findings. If Program is inaccessible, check the public
partial declaration was saved at the end of Program.cs. Share restore,
runtime, discovery, or test errors before proceeding. Backend checks have
now passed per owner-provided results. TECH-001 remains open until all
criteria and the owner-created code/documentation commit are verified.
Add a unit-test project when isolated business logic exists; avoid empty tests.

Reference: https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-10.0
