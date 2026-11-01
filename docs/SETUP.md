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
First-computer Docker engine operation is also unverified.

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
