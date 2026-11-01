# Development Setup

## Status
Foundation stage only. No application projects or services exist yet.
Commands below are instructions for the owner, not actions performed by Codex.
Second-computer execution has not yet been verified.

## FND-003 — Two-computer continuity
Objective: retrieve the same committed project context on both computers
and inventory the tools needed for subsequent application setup.
Prerequisites: Windows 11, PowerShell, Git, GitHub repository access,
and the latest documentation committed and pushed from the first computer.

## First computer: publish the setup guide
Run from the Calliqo repository. Review the documentation before committing.

```powershell
git diff -- docs/DEVELOPMENT_STATE.md docs/ROADMAP.md
Get-Content docs/SETUP.md
git add docs/DEVELOPMENT_STATE.md docs/ROADMAP.md docs/SETUP.md
git diff --cached --check
git commit -m "docs: establish two-computer setup workflow"
git push
git status --short --branch
git rev-parse HEAD
```

Record the full resulting commit ID. Do not compare the second computer
against an older commit from a previous session.

## Second computer: obtain the repository
Choose an existing parent directory for repositories. If Calliqo is not
already present, clone it there:

```powershell
git clone https://github.com/Ravynnox/Calliqo.git
Set-Location Calliqo
```

If Calliqo already exists, enter it and inspect before updating:

```powershell
git status --short --branch
git remote -v
git branch --show-current
```

If changes exist, the remote differs, or the branch is not main, stop and
review before proceeding. With a clean main tracking origin/main:

```powershell
git fetch origin
git pull --ff-only
```

A failed fast-forward requires review; do not reset or force-push.

## Verify retrieval and tools
Run from the repository on the second computer:

```powershell
git status --short --branch
git rev-parse HEAD
git log -1 --oneline
git remote -v
git --version
dotnet --version
node --version
npm --version
docker --version
docker compose version
docker info
```

The full commit ID must match the first computer's recorded ID.
The working tree must be clean. Docker info must successfully contact
the engine; CLI version output alone does not verify Docker Desktop.
If a command is unavailable or fails, record the error for guided setup.
Report only results needed for diagnosis; redact sensitive configuration.

Observed first-computer versions on 2026-11-01:
Git 2.56.0.windows.1; .NET SDK 10.0.201; Node.js 20.20.1;
npm 10.8.2; Docker 29.9.2; Compose v5.6.1.
These are inventory results, not approved version requirements.
Version compatibility and reproducible version pinning remain to be
established before application scaffolding.
Latest owner-provided inventory: Node v26.13.0; npm 11.20.0;
.NET SDK 10.0.201. An initial Docker engine failure was subsequently
resolved: direct verification returned Server 29.9.2, linux.

## Application preparation — current computer
The owner requested preparing application setup while second-computer
verification remains pending. This does not complete FND-003.

Node 20 is end-of-life according to the official release table:
https://nodejs.org/en/about/previous-releases
Node 24 was initially recommended. The owner subsequently installed
Node v26.13.0, and the official release table lists Node 26 as LTS.
Keep the installed version for compatibility validation; no downgrade is
needed solely to match the earlier recommendation. Exact project version
requirements have not yet been pinned.
Official Windows installers are available at:
https://nodejs.org/en/download
After installation, restart the VS Code terminal and verify:

```powershell
node --version
npm --version
dotnet --list-sdks
docker info
```

Expected: a supported Node LTS version, npm runs successfully, a .NET 10 SDK is listed,
and Docker info includes server information without an engine error.
If Node still reports v20, restart VS Code and inspect Get-Command node.
Do not generate application projects until tool versions and scaffold
commands are reviewed. No installation or scaffolding has been performed
by Codex.

## Docker engine startup troubleshooting
The owner's latest output shows desktop-linux as the context, but reports
that the dockerDesktopLinuxEngine named pipe cannot be found. This means
the CLI cannot reach that engine; the exact startup cause is not established.

Open Docker Desktop from the Windows Start menu and wait for the engine
to finish starting, then run docker info again. Success includes populated
Server information, not just the Client section.

If the engine remains unavailable, report the Docker Desktop startup error
and run these read-only diagnostics in PowerShell:

```powershell
wsl --status
wsl --version
docker context ls
```

Review results before changing WSL, Docker settings, or reinstalling.
Reference: https://docs.docker.com/desktop/features/wsl/

## Recover the development context
Open the cloned folder in Visual Studio Code. Start a Codex conversation
with “Let's continue where we left off.” Confirm that Codex reads AGENTS.md,
development state, roadmap, and architecture before proposing work.

## Acceptance criteria
- Both computers retrieve the same full commit ID with clean working trees.
- Tool inventories and outstanding setup problems are recorded.
- Docker engine availability is verified on both computers, or documented
  as an unresolved setup issue rather than claimed as working.
- A new session identifies the current task from repository documentation.
- No application code is generated during this task.

After verification, update development state with actual evidence.
The owner commits and pushes the final handoff documentation.

## TECH-001 — Frontend scaffold walkthrough
Status: scaffold and dependencies exist. Owner-provided output confirms
production build and lint pass after the TypeScript fix below.
Owner subsequently confirmed browser startup and interaction worked.
Owner executes these commands.
Second-computer verification remains pending while local setup proceeds.

From the repository root, generate the React TypeScript template:

```powershell
npm create vite@latest client -- --template react-ts
```

Accept the create-vite download if prompted. Choose the standard stable
React/TypeScript template; decline experimental features. If offered
automatic installation/startup, decline and use the explicit steps below.
Stop if client already exists and contains work; do not overwrite it.
The latest generator is resolved at execution time. Record the generated
package versions and retain package-lock.json for subsequent npm ci installs.

```powershell
Set-Location client
npm install
npm run build
npm run lint
npm run dev
```

Open the local URL Vite prints. Acceptance: the starter screen renders,
build and lint exit successfully, and no engine compatibility warning is
left unresolved. Use Ctrl+C to stop the server, then Set-Location .. to
return to the repository root.

Generated files belong under client/, including package.json, lockfile,
TypeScript/Vite configuration, index.html, and src/ starter files.
Vite builds to client/dist, which should remain ignored along with
client/node_modules. No backend communication exists at this step.
Report errors before proceeding. Backend scaffolding and baseline test
setup follow after this frontend checkpoint. No automated unit tests have
been added yet. Review Git changes before any owner-controlled commit.

Reference: https://vite.dev/guide/

## Frontend dependency conflict — create-vite 9.3.3
The owner selected ESLint and automatic installation. The generated
client/package.json requests TypeScript ~7.0.2 and typescript-eslint
^8.72.0. Installation failed: resolved typescript-eslint 8.72.1 requires
TypeScript >=4.8.4 <6.1.0. No successful install or build is verified.
Official compatibility reference:
https://typescript-eslint.io/users/dependency-versions/

Proposed owner-run fix from the repository root:

```powershell
Set-Location client
npm install --save-dev --save-exact "typescript@~6.0.0"
npm ls typescript typescript-eslint
npm run build
npm run lint
```

This selects a stable 6.0.x release within the linter's supported range,
records the exact resolved compiler version in client/package.json, and
creates/updates client/package-lock.json on successful installation.
Do not bypass the conflict with --force or --legacy-peer-deps.
Keep existing scaffold files; do not rerun the generator over client/.
After successful checks, run npm run dev and verify the starter page.
Report any further error rather than assuming the fix is verified.

### Verified recovery results
Owner-provided output confirms TypeScript 6.0.3 with typescript-eslint
8.72.1, successful tsc -b and Vite 8.3.5 production build, and successful
ESLint execution. File inspection confirms TypeScript 6.0.3 is pinned and
package-lock.json exists. node_modules and dist are ignored by Git.
The frontend remains uncommitted. The owner subsequently confirmed the
browser check passed; automated tests remain outstanding.

## TECH-001 — Backend scaffold walkthrough
Status: solution and API project now exist. Owner output confirms startup
on http://localhost:5033, with HTTPS and dependency warnings noted below.
API response and clean build have not yet been verified.

From the repository root, create the solution and .NET 10 API project:

```powershell
dotnet new sln --name Calliqo --format slnx
dotnet new webapi --name Calliqo.Api --output server/Calliqo.Api --framework net10.0
dotnet sln Calliqo.slnx add server/Calliqo.Api/Calliqo.Api.csproj
dotnet build Calliqo.slnx
```

Calliqo.slnx groups backend projects and later tests. The API template uses
Minimal APIs by default, which can later be grouped into module endpoint
files. Program.cs configures the server and initial endpoints; the csproj
declares the target framework and dependencies. Template configuration and
Properties/launchSettings.json hold initial settings and local run profiles.
No database or authentication is configured by this step.

Start the generated HTTP development profile:

```powershell
dotnet run --project server/Calliqo.Api --launch-profile http
```

Use the HTTP base URL printed after 'Now listening on' and append
/weatherforecast in a browser. Expected: generated sample JSON.
A 404 at the root URL is normal because the template has no home page.
Keep the server running while checking the endpoint, then stop with Ctrl+C.
The HTTP profile is for local verification; cloud delivery requires HTTPS.

Acceptance: solution build succeeds and sample endpoint responds.
If restore fails or a port is occupied, share the exact error before
changing generated files. Baseline tests and version requirements remain
part of TECH-001 after this checkpoint. The owner reviews, commits, and
pushes only after verification and documentation updates.

### Backend startup checks and dependency warning
The generated Program.cs has no root endpoint. Use
http://localhost:5033/weatherforecast for sample JSON and
http://localhost:5033/openapi/v1.json for the Development OpenAPI document.
Root 404 is expected. The HTTP launch profile starts no HTTPS listener,
so UseHttpsRedirection reports that it cannot determine the HTTPS port.
That warning is separate from the root 404.

Owner output reports NU1903 for transitive Microsoft.OpenApi 2.0.0.
The project directly references Microsoft.AspNetCore.OpenApi 10.0.5.
Advisory https://github.com/advisories/GHSA-v5pm-xwqc-g5wc identifies
2.7.5 as patched on the 2.x line. Proposed owner-run remediation from
the repository root, after stopping the API with Ctrl+C:

```powershell
dotnet add server/Calliqo.Api/Calliqo.Api.csproj package Microsoft.OpenApi --version 2.7.5
dotnet build Calliqo.slnx
dotnet list server/Calliqo.Api/Calliqo.Api.csproj package --include-transitive --vulnerable
dotnet run --project server/Calliqo.Api --launch-profile http
```

This adds an explicit patched transitive dependency version to the project.
It has not yet been applied or verified. Check the build/audit results and
verify both endpoints after restarting; do not suppress NU1903 as a fix.
If the audit cannot reach its sources, it is inconclusive, not a clean result.

Latest evidence: owner screenshot confirms /weatherforecast returns JSON
before the dependency update. Project inspection now confirms the explicit
Microsoft.OpenApi 2.7.5 reference. Owner-provided output confirms the
solution build succeeds and the transitive vulnerability audit reports
no vulnerable packages using api.nuget.org. The audit is limited to known
advisories from the current sources. The owner subsequently confirmed
OpenAPI and weatherforecast both respond after restarting with the updated
dependency. Baseline test setup and version pinning remain outstanding.

## TECH-001 — Version requirements walkthrough
Status: proposed owner edits; none of the configuration below has been
applied or verified. Prerequisites: scaffold checks passed, Node 26.13.0,
npm 11.20.0, .NET SDK 10.0.201. FND-004 is complete.

Create global.json at the repository root (JSON; uses .NET SDK resolver):

```json
{
  "sdk": {
    "version": "10.0.201",
    "rollForward": "latestPatch",
    "allowPrerelease": false
  }
}
```

This selects the latest installed stable 10.0.2xx SDK patch at or above
10.0.201, not another feature band or major. It allows patch maintenance
while keeping the feature band consistent. It does not install an SDK.

Create .node-version at the repository root (plain text):

```text
26.13.0
```

This records the expected Node version for developers and compatible
version managers; it does not switch Node automatically in PowerShell.

From the repository root, update client/package.json using npm:

```powershell
Set-Location client
npm pkg set 'engines.node=>=26.13.0 <27' 'engines.npm=>=11.20.0 <12' 'packageManager=npm@11.20.0'
npm install --package-lock-only
npm ci
npm run build
npm run lint
Set-Location ..
dotnet --version
dotnet build Calliqo.slnx
```

The npm command records supported major ranges and the npm baseline.
engines is advisory by default; packageManager is metadata and does not
automatically install npm. Retain package-lock.json; npm ci reproduces its
dependency versions. The lock-only step synchronizes manifest metadata
before the clean dependency install. Check resulting file diffs.
Expected: selected .NET SDK matches global.json, npm install/ci succeeds,
frontend build/lint and backend build pass. Share errors before proceeding.
No baseline tests are added by this step; TECH-001 stays open.

References:
- https://learn.microsoft.com/en-us/dotnet/core/tools/global-json
- https://docs.npmjs.com/cli/v11/configuring-npm/package-json/#engines

### Version requirements verification and npm audit follow-up
Owner completed the walkthrough. Direct inspection confirms global.json,
.node-version, and client engines/packageManager metadata. Owner-provided
output confirms npm ci, frontend build/lint, dotnet --version (10.0.201),
and solution build passed.

Full read-only npm audit reports 12 moderate affected-package findings
rooted in minimatch 10.2.6 through ESLint 10.14.0 and typescript-eslint
8.72.1. Advisory GHSA-g3hv-75wg-m249 identifies versions below 11.0.1
as affected. Read-only npm audit --omit=dev reports zero known findings.
Do not equate a production-only clean report with a clean full toolchain.
No npm audit fix, dependency override, downgrade, or upgrade was applied.
Review compatible fixes before choosing remediation or explicit deferral.
Reference: https://github.com/advisories/GHSA-g3hv-75wg-m249

The agent's default npm audit request failed certificate verification;
using Node --use-system-ca for that process allowed verified TLS requests.
No persistent trust settings or execution policies were changed.
Version/scaffold changes remain uncommitted. Baseline frontend and backend
tests have subsequently passed; see TESTING.md for the current evidence.

### Minimatch override trial — applied and locally verified
Registry review found latest eslint 10.14.0 depends on minimatch ^10.2.6,
@eslint/config-array 0.23.5 on ^10.2.4, and
@typescript-eslint/typescript-estree 8.72.1 on ^10.2.2.
No normal latest-version update resolves the current advisory.
minimatch 11.0.1 supports the installed Node version, but the 11.x release
changes matching internals and removes APIs. An override crosses declared
major ranges; local validation is required and does not establish upstream support.

Proposed owner commands from client/ modify client/package.json and its lockfile:

```powershell
npm pkg set 'overrides.eslint.minimatch=11.0.1' 'overrides.@eslint/config-array.minimatch=11.0.1' 'overrides.@typescript-eslint/typescript-estree.minimatch=11.0.1'
npm install
npm ci
npm ls minimatch
npm audit
npm run build
npm run lint
npx eslint src/App.tsx src/main.tsx vite.config.ts --max-warnings 0
```

Expected: three consumers resolve patched 11.0.1; full audit, build, and
lint checks pass without ignored-file warnings. Check rule execution using
a temporary stdin input rather than creating a repository file:

```powershell
'const unusedProbe = 1;' | npx eslint --stdin --stdin-filename src/lint-probe.ts --max-warnings 0
```

This final probe is expected to fail with an unused-variable error. That
failure confirms matching/rule execution for the virtual TypeScript file.
If the real lint checks fail or the probe unexpectedly succeeds, stop and
report the output. Do not claim remediation is verified from audit alone.
If the trial fails, remove only the three proposed override entries and
reinstall; do not discard the owner's other package or lockfile changes.
When upstream consumers support the patched major, review removing these
temporary overrides. No package files have been changed by Codex.

Verification results: owner applied the three overrides; direct manifest
inspection confirms them. Owner output shows npm install/ci and full audit
report zero findings, all consumers resolve 11.0.1, build/lint pass, and
explicit source/config lint checks pass. Agent ran the stdin-only probe:
expected exit 1 with @typescript-eslint/no-unused-vars. No source file was
created. These checks validate the current workflow, not all upstream API
compatibility. Review the override when lint dependencies/configuration change.
