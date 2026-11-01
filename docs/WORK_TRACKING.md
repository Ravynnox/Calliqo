# GitHub Work Tracking

## Current status
- Current task: [FND-004 — GitHub work tracking](https://github.com/Ravynnox/Calliqo/issues/4).
- Owner authorized setup before version pinning; authenticated GitHub CLI as Ravynnox.
- Repository: https://github.com/Ravynnox/Calliqo
- Private Project: [Calliqo Development](https://github.com/users/Ravynnox/projects/2), linked to the repository.
- Workflow board created with Backlog, Ready, In Progress, and Done statuses.
- Published 24 real GitHub issues after confirming no existing repository issues.
- Local body snapshots live under docs/work-items/; task/issue mapping is below.
- All 24 memberships are verified. FND-004 is now closed/Done, giving
  3 Done, 2 In Progress, and 19 Backlog items; 21 issues remain open.
- Workflow BOARD_LAYOUT and Status column grouping were verified through
  verticalGroupByFields. groupByFields alone does not report board columns.
- GitHub connector/plugin discovery were unavailable; authenticated GitHub CLI is the verified fallback.
- Tracking documentation is committed and published in d01167c.
- Current completion updates are local; include them in the next checkpoint.

## Responsibilities
The owner controls source/configuration changes, branches, staging, commits,
pushes, and merges. Codex maintains repository documentation automatically.
Remote issue/Project writes require authorization; the current FND-004 setup
is authorized. Ask for authorization for later remote tracking changes if
not already covered by the user's instructions.

GitHub Issues hold work items. Projects organizes those issues visually.
The repository retains the durable roadmap, architecture, decisions, and
session handoff. Keep the issue links and state aligned with verified work.
Do not require an extra documentation commit after every status observation;
include handoff updates in the next meaningful owner-reviewed checkpoint.

## Task identities and issue content
Keep stable IDs such as TECH-001 in titles and documentation. GitHub assigns
separate issue numbers; never assume TECH-001 is issue #1.
Each issue uses its draft's objective, dependencies, acceptance criteria,
evidence, and completion record. Link dependencies by real issue number once
publication establishes the mapping.

## Board layout
Use a Board view grouped by Status:
- Backlog: planned work, including unmet dependencies.
- Ready: dependencies satisfied and owner approved starting the task.
- In Progress: active work or unfinished work with a clear pending note.
- Done: acceptance criteria met, evidence recorded, and required code/docs committed.

FND-001, FND-002, and FND-004 are Done. FND-003 remains In Progress with
second-computer verification pending. TECH-001 is active for version pinning
and baseline tests. All other tasks remain in Backlog.
Use a note in the issue body for waiting work; no additional custom field is
required initially.

## CLI setup reference — already completed on the first computer
Install GitHub's official CLI using Windows Package Manager:

```powershell
winget install --id GitHub.cli --exact --source winget
```

Restart the VS Code terminal (restart VS Code if PATH is still stale), then:

```powershell
gh --version
gh auth login --hostname github.com --git-protocol https --web --scopes project
gh auth status
gh repo view Ravynnox/Calliqo --json nameWithOwner,url,viewerPermission
```

Authenticate as an account with write access to Ravynnox/Calliqo. The project
scope enables Projects operations. Browser authorization is performed by the
owner; never paste access tokens into chat or repository files.
If an existing gh login lacks Projects scope, use:

```powershell
gh auth refresh --hostname github.com --scopes project
```

Installation/authentication is now complete on the first computer. These commands
remain a reference for another computer; do not repeat setup unnecessarily.

## Publication procedure after access is verified
1. List all existing repository issues, including closed issues, and owner
   projects. Match tasks by stable IDs to avoid duplicates.
2. Create or reuse the owner's Calliqo Development project and link it to
   Ravynnox/Calliqo. Keep the Project private initially unless the owner
   requests otherwise; do not change repository visibility.
3. Publish/reuse one issue per draft. Use body files to preserve Markdown.
4. Record each real issue number and URL in this index and the roadmap.
5. Replace dependency IDs in remote issue bodies with links to the created issues.
6. Add every issue to the Project and set initial statuses.
7. Close only the verified historical FND-001/FND-002 issues as completed.
8. Verify issue count, board membership/statuses, and links. Do not claim
   the board is ready before those checks succeed.
9. Owner reviews, commits, and pushes the documentation, with the FND-004
   issue reference. Resume TECH-001 only after the workflow is established.

Reference inspection commands from the repository root:

```powershell
gh issue list --repo Ravynnox/Calliqo --state all --limit 200 --json number,title,url,state
gh project list --owner Ravynnox --format json
```

Creation example for reference only: TECH-001 is already issue #5; do not run
this command again for the same task:

```powershell
gh issue create --repo Ravynnox/Calliqo --title "TECH-001 — Application scaffolds" --body-file docs/work-items/TECH-001.md
```

Do not run this creation example repeatedly. Use the returned issue number
for subsequent updates. Closed historical tasks retain accurate completion
evidence; new work must not be marked complete to populate the board.

## Commit and pull-request conventions
TECH-001 is [issue #5](https://github.com/Ravynnox/Calliqo/issues/5):
- Branch: tech-001-application-scaffolds.
- Partial commit: TECH-001: add frontend and API scaffolds (#5).
- PR title: TECH-001: establish application scaffolds.
- Partial PR description: Refs #5; list remaining acceptance criteria.
- Final PR description: Closes #5 only when the task's acceptance criteria
  are met. Automatic closure applies when merged into the default branch.

The owner creates branches and commits. Do not move the current uncommitted
scaffold automatically. A scaffold checkpoint may reference TECH-001 while
leaving its issue open for version pinning and baseline tests.
A bare TECH-001 identifier is not a GitHub issue reference; include #NUMBER
or the actual issue URL. Ordinary #NUMBER references associate work without
requesting closure. Do not use closing keywords for partial work.

## Session handoff
Read AGENTS.md, DEVELOPMENT_STATE.md, ROADMAP.md, this index, and the active
issue. Check local/remote Git state. Record actual test results and gaps.
Issue/Project state does not override uncommitted local work or prove remote
synchronization. Commit hashes describe the last verified checkpoint; they
need not be rewritten into a new self-referential commit after each commit.

## Work-item index

| Task ID | Local issue body snapshot | Roadmap status | GitHub issue |
|---|---|---|---|
| FND-001 | [Foundation review](work-items/FND-001.md) | Complete | [#1](https://github.com/Ravynnox/Calliqo/issues/1) |
| FND-002 | [Repository memory](work-items/FND-002.md) | Complete | [#2](https://github.com/Ravynnox/Calliqo/issues/2) |
| FND-003 | [Two-computer setup](work-items/FND-003.md) | In progress | [#3](https://github.com/Ravynnox/Calliqo/issues/3) |
| FND-004 | [GitHub work tracking](work-items/FND-004.md) | Complete | [#4](https://github.com/Ravynnox/Calliqo/issues/4) |
| TECH-001 | [Application scaffolds](work-items/TECH-001.md) | In progress | [#5](https://github.com/Ravynnox/Calliqo/issues/5) |
| TECH-002 | [API/database connection](work-items/TECH-002.md) | Planned | [#6](https://github.com/Ravynnox/Calliqo/issues/6) |
| TECH-003 | [API conventions and CI](work-items/TECH-003.md) | Planned | [#7](https://github.com/Ravynnox/Calliqo/issues/7) |
| OBS-001 | [Baseline telemetry](work-items/OBS-001.md) | Planned | [#8](https://github.com/Ravynnox/Calliqo/issues/8) |
| AUTH-001 | [Identity decision](work-items/AUTH-001.md) | Planned | [#9](https://github.com/Ravynnox/Calliqo/issues/9) |
| AUTH-002 | [Sign-in and teams](work-items/AUTH-002.md) | Planned | [#10](https://github.com/Ravynnox/Calliqo/issues/10) |
| AUTH-003 | [Team isolation](work-items/AUTH-003.md) | Planned | [#11](https://github.com/Ravynnox/Calliqo/issues/11) |
| DOM-001 | [Football domain](work-items/DOM-001.md) | Planned | [#12](https://github.com/Ravynnox/Calliqo/issues/12) |
| PLAY-001 | [Structured play contract](work-items/PLAY-001.md) | Planned | [#13](https://github.com/Ravynnox/Calliqo/issues/13) |
| PLAY-002 | [Playbook persistence](work-items/PLAY-002.md) | Planned | [#14](https://github.com/Ravynnox/Calliqo/issues/14) |
| EDIT-001 | [Renderer evaluation](work-items/EDIT-001.md) | Planned | [#15](https://github.com/Ravynnox/Calliqo/issues/15) |
| EDIT-002 | [Interactive editor](work-items/EDIT-002.md) | Planned | [#16](https://github.com/Ravynnox/Calliqo/issues/16) |
| GAME-001 | [Event contract](work-items/GAME-001.md) | Planned | [#17](https://github.com/Ravynnox/Calliqo/issues/17) |
| GAME-002 | [Game-day workflow](work-items/GAME-002.md) | Planned | [#18](https://github.com/Ravynnox/Calliqo/issues/18) |
| GAME-003 | [Corrections/statistics](work-items/GAME-003.md) | Planned | [#19](https://github.com/Ravynnox/Calliqo/issues/19) |
| CLOUD-001 | [Hosting/IaC decision](work-items/CLOUD-001.md) | Planned | [#20](https://github.com/Ravynnox/Calliqo/issues/20) |
| INFRA-001 | [Cloud development environment](work-items/INFRA-001.md) | Planned | [#21](https://github.com/Ravynnox/Calliqo/issues/21) |
| OBS-002 | [Grafana dashboards and alerts](work-items/OBS-002.md) | Planned | [#22](https://github.com/Ravynnox/Calliqo/issues/22) |
| INFRA-002 | [Production deployment and recovery](work-items/INFRA-002.md) | Planned | [#23](https://github.com/Ravynnox/Calliqo/issues/23) |
| REL-001 | [MVP verification](work-items/REL-001.md) | Planned | [#24](https://github.com/Ravynnox/Calliqo/issues/24) |

## Acceptance and remaining work
CLI setup, workflow documentation, issue creation, and Project creation are complete.
Project membership/status verification, saved Status column grouping, and
documentation publication passed. FND-004 is closed and Done. Resume TECH-001.

## Official references
- [GitHub CLI installation](https://github.com/cli/cli#installation)
- [GitHub CLI Projects](https://cli.github.com/manual/gh_project)
- [Creating a Project](https://docs.github.com/en/issues/planning-and-tracking-with-projects/creating-projects/creating-a-project)
- [Adding Project items](https://docs.github.com/en/issues/planning-and-tracking-with-projects/managing-items-in-your-project/adding-items-to-your-project)
- [Linking issues and pull requests](https://docs.github.com/en/issues/tracking-your-work-with-issues/using-issues/linking-a-pull-request-to-an-issue)

