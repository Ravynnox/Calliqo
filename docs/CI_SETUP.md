# CI setup walkthrough — TECH-003 (#7)

> Review 2026-11-02: this workflow is implemented at .github/workflows/ci.yml, committed in a77b651, and passed both jobs in run 41551585014. Earlier missing-path, create-file, and pending-publication instructions below are historical. No workflow recreation or relocation is needed.

## Status and objective
Prepared 2026-11-02 for owner implementation. Workflow now exists at the correct root path and has been reviewed;
publication and remote CI execution remain pending. Owner approved continuing TECH-003 guidance.
Codex changes documentation only. The weather-validation exercise is cancelled;
real request validation is tracked in AUTH-002 (#10).

Goal: GitHub runs the frontend checks and backend tests on main pushes and pull
requests targeting main, using repository tool version files and isolated tests.

## Prerequisites
- Existing frontend test/build/lint and four backend tests pass per owner results.
- client/package-lock.json, .node-version, global.json, and solution are tracked.
- GitHub Actions is enabled for the repository. No application secrets are needed.
- Owner reviews the pending API/test/documentation edits with the workflow.

## Create .github/workflows/ci.yml
Destination: repository-root .github/workflows/ci.yml. Language: YAML.
Purpose: two independent jobs, with steps in each job running sequentially.
Use spaces for indentation. Create the directories if they do not exist.

```yaml
name: CI

on:
  push:
    branches: [main]
  pull_request:
    branches: [main]
  workflow_dispatch:

permissions:
  contents: read

concurrency:
  group: ${{ github.workflow }}-${{ github.ref }}
  cancel-in-progress: true

jobs:
  frontend:
    name: Frontend checks
    runs-on: ubuntu-24.04
    timeout-minutes: 15
    defaults:
      run:
        working-directory: client
    steps:
      - name: Check out repository
        uses: actions/checkout@3d3c42e5aac5ba805825da76410c181273ba90b1 # v7.0.1
        with:
          persist-credentials: false

      - name: Set up Node.js
        uses: actions/setup-node@949feb2413d6458794dcd2491c4babbbce0c15c1 # v7.1.0
        with:
          node-version-file: .node-version
          cache: npm
          cache-dependency-path: client/package-lock.json

      - name: Match repository npm version
        run: npm install --global npm@11.20.0

      - name: Show tool versions
        run: |
          node --version
          npm --version

      - name: Install frontend dependencies
        run: npm ci

      - name: Lint frontend
        run: npm run lint -- --max-warnings 0

      - name: Test frontend
        run: npm run test

      - name: Build frontend
        run: npm run build

  backend:
    name: Backend checks
    runs-on: ubuntu-24.04
    timeout-minutes: 20
    env:
      DOTNET_NOLOGO: true
    steps:
      - name: Check out repository
        uses: actions/checkout@3d3c42e5aac5ba805825da76410c181273ba90b1 # v7.0.1
        with:
          persist-credentials: false

      - name: Set up .NET
        uses: actions/setup-dotnet@a98b56852c35b8e3190ac28c8c2271da59106c68 # v6.0.0
        with:
          global-json-file: global.json

      - name: Show SDK and verify Docker
        run: |
          dotnet --version
          docker info

      - name: Restore backend dependencies
        run: dotnet restore Calliqo.slnx

      - name: Build backend
        run: dotnet build Calliqo.slnx --configuration Release --no-restore

      - name: Test backend
        run: dotnet test Calliqo.slnx --configuration Release --no-build --no-restore
```

## Walkthrough and tradeoffs
- Pushes to main and pull requests targeting main run both jobs. Feature-branch
  pushes without an open PR do not trigger this configuration. Manual dispatch
  is usable after the workflow exists on the default branch.
- A failed step fails its job. Frontend and backend run independently, so one
  can still provide feedback if the other fails. A newer run cancels obsolete
  work on the same ref. This is CI, not deployment or cloud provisioning.
- The setup actions read paths relative to the workspace. The run working-directory
  setting only changes shell steps, so .node-version stays a root-relative path.
- Node comes from .node-version (currently 26.13.0). npm is explicitly installed
  at 11.20.0 to match client/package.json; update that workflow line when changing
  the repository npm pin. npm ci uses the committed dependency lockfile.
- .NET setup reads global.json (currently 10.0.201 with latestPatch policy).
  Setup-dotnet v6 honors that roll-forward policy, so the selected SDK can be a
  newer patch in the permitted feature band. Read the logged version; do not
  describe the workflow as enforcing a single immutable SDK patch.
- Full commit SHAs pin action implementations. Release comments identify the
  versions verified against official release pages on 2026-11-02. Review pins
  when upgrading actions. ubuntu-24.04 fixes the OS family, but runner images,
  package registries, and Docker tags still evolve; this is not a hermetic build.
- npm caches downloaded packages, not node_modules. NuGet caching is omitted
  initially because there are no packages.lock.json files; no lockfile or cache
  infrastructure is added solely for this first workflow.
- GitHub-hosted Ubuntu provides Docker. Testcontainers starts PostgreSQL 18.6
  on a dynamically assigned port and cleans it up. No services stanza, Compose
  startup, host port 5432, local .env, or development connection string is needed.
  Do not skip persistence tests or disable the resource reaper to hide failures.
- API tests use the in-process HTTPS test client. OpenApiTests explicitly sets
  Development. PersistenceTests supplies its isolated container connection directly;
  DbContext registration defers reading configuration until it is resolved.
- contents: read and persist-credentials: false are sufficient for checkout and
  these checks. No custom GitHub token, registry credential, production secret,
  privileged PR trigger, or local secrets file belongs in this workflow.
- This runs tests/lint/build. It does not configure branch protection, deploy,
  perform recovery tests, or establish a comprehensive vulnerability policy.

## Local review and validation
First save the YAML and ask Codex to inspect it before committing. From repository
root, these PowerShell commands review changes without staging anything:

```powershell
git diff --check
git status --short
Get-Content .github/workflows/ci.yml
```

A new untracked workflow is not shown by ordinary git diff until tracked;
Get-Content provides its initial review. Existing local test results do not prove
this YAML will execute successfully on GitHub. No need to repeat all previously
passing local tests solely because a workflow was added; remote Linux execution
will resolve the remaining platform/tool-setup uncertainty.

## Owner Git guidance and remote verification
After reviewing source, tests, YAML, and documentation, owner creates their
checkpoint/PR using the established workflow. Suggested commit/PR title:
TECH-003: add API conventions and continuous integration (#7).
Include all intended TECH-003 API/test/docs changes, not only the YAML. Include
the authorized tracking updates to AUTH-002 docs. Keep .env and User Secrets out.
Use Refs #7, not Closes #7, until the acceptance evidence is verified.
If using a PR, branch creation and any transfer of current uncommitted changes
remain owner actions. Do not change the current working branch automatically.

After pushing main or opening a PR targeting main, inspect GitHub Actions or
run these read-only PowerShell commands from repository root:

```powershell
gh run list --repo Ravynnox/Calliqo --workflow ci.yml --limit 5
```

Then use the actual run ID from the list with gh run view or the GitHub run
page. Share that run URL or failed-step output; never paste credentials.
Do not assume an old successful run covers the new commit.

## Acceptance criteria and next action
- GitHub accepts this workflow and both Frontend checks and Backend checks pass
  for the intended commit. Expected currently: one frontend test, four backend
  tests, frontend lint/build, and backend Release build.
- Backend logs confirm Docker-backed persistence executes successfully.
- No application secrets or local environment files are required/exposed.
- Associated source, tests, and documentation are owner-committed/published.
- Record the run URL and commit, then reconcile TECH-003 (#7) completion.
  Checks running does not automatically make them required merge checks;
  branch protection/rulesets would need a separate owner-approved configuration.

Current action: owner creates the YAML and shares it for inspection. Implementation,
remote execution, and completion remain unverified. No workflow/configuration,
Git, or external tracking mutation was performed by Codex for this walkthrough.

## Official references
- [Checkout v7.0.1 release](https://github.com/actions/checkout/releases/tag/v7.0.1)
- [Setup Node v7.1.0 release](https://github.com/actions/setup-node/releases/tag/v7.1.0)
- [Setup Node inputs and caching](https://github.com/actions/setup-node)
- [Setup .NET v6.0.0 release](https://github.com/actions/setup-dotnet/releases/tag/v6.0.0)
- [Setup .NET global.json and roll-forward behavior](https://github.com/actions/setup-dotnet)
- [Testcontainers on CI](https://dotnet.testcontainers.org/cicd/)

## Owner review checkpoint — 2026-11-02
Owner created docs/.github/workflows/ci.yml; direct inspection confirms its
content matches the guide. Repository-root .github/workflows/ci.yml is absent.
GitHub cannot discover the nested docs/.github path. Owner must relocate it
before publication. No YAML execution or remote CI success is claimed.

## Placement correction verified — 2026-11-02
.github/workflows/ci.yml now exists and is the only ci.yml found. Its content
matches the walkthrough. Git diff --check passes. Prior misplaced-path notes
are historical. Workflow is untracked; owner staging/publication is next.
No remote CI result or local test rerun is claimed.

## Remote result verified — 2026-11-02
Published checkpoint a77b651b655bc3301e8b77549b3c77ec179efe42 passed both jobs
in [run 41551585014](https://github.com/Ravynnox/Calliqo/actions/runs/41551585014).
Frontend install/lint/test/build passed (one test). Backend Release build had
zero warnings/errors; four tests passed, zero failed/skipped. Docker check
passed. CI selected SDK 10.0.204 through latestPatch; Node 26.13.0 and npm
11.20.0 were logged. Initial pending/placement notes above are historical.
Only main-push execution is verified; no PR-trigger or branch-protection claim.
