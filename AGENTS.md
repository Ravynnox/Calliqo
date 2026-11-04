# Calliqo — Working Instructions

## Purpose
Build a production-quality, mobile-first American football coaching
web application while teaching the owner how to develop and maintain it.

Support configurable football formats, including 5v5, 7v7, 9-player,
and 11-player football.

## Authorization and teaching
- Default to mentorship: read, explain, guide, verify.
- Documentation may be created and updated automatically to maintain
  accurate project instructions, decisions, plans, and development state.
  No separate authorization is needed for documentation edits.
- The owner controls code, configuration, infrastructure, and Git changes.
  Do not modify non-documentation files without explicit authorization.
  Documentation permission does not authorize changes to scripts, CI,
  dependency manifests, .gitignore, or other executable/configuration files.
- Safe read-only inspection is permitted.
- “Continue” and similar instructions do not authorize implementation.
- Implementation authorization applies only to the specified task.
- Return to mentorship mode after authorized implementation.
- The owner performs Git mutations. Do not stage, commit, push, initialize
  repositories, configure remotes, merge, reset, or change/delete branches
  unless explicitly asked to perform that specific action.
- Explain decisions, tradeoffs, commands, and validation.
- For every task and every document, present complete actionable walkthroughs,
  code, commands, destinations, and validation directly in chat. Documentation
  is reference and project memory; never require the owner to discover steps there.
- Every code snippet must identify its destination and purpose.
- Obtain approval before proceeding to the next development task.
- Never claim implementation, testing, or synchronization without evidence.

## Required stack
Frontend: React, TypeScript, Vite, React Router, TanStack Query,
Tailwind CSS, shadcn/ui, React Hook Form, and Zod.

Backend: .NET 10, ASP.NET Core REST API, EF Core, Npgsql, PostgreSQL,
dependency injection, structured logging, OpenAPI, and OpenTelemetry.
Use FluentValidation where appropriate.

Testing: xUnit, Vitest, React Testing Library, Playwright, and
Testcontainers as relevant to each task.

Infrastructure: Docker, Docker Compose, Git, GitHub, and GitHub Actions
when appropriate. Cloud provider and infrastructure-as-code tool remain
undecided.

## Architecture
- Use a modular monolith with clear module ownership.
- Store editable plays as versioned, structured football data.
- Keep renderer-specific objects out of the persisted domain contract.
- Derive game statistics from recorded events and audited corrections.
- Enforce permissions and team isolation server-side.
- Design for mobile, tablet, mouse, touch, and stylus use.
- Keep AI optional, behind backend services, and outside the initial MVP.
- Introduce abstractions and dependencies only when justified.

## MVP scope
1. Secure sign-in and authorized team access.
2. Structured playbooks and editable football diagrams.
3. Basic game recording, corrections, and derived statistics.

Defer image import, scouting, AI recommendations, billing, integrations,
advanced analytics, and full offline synchronization.

## Cloud and operations
- Plan for cloud hosting without selecting a provider prematurely.
- Provision the first cloud environment through reviewed infrastructure
  as code.
- Protect infrastructure state and secrets.
- Plan separate environments, database migrations, backups, and recovery.
- Instrument with OpenTelemetry and plan Grafana dashboards and alerts.
- Choose metric, log, and trace storage during hosting evaluation.
- Keep secrets and unnecessary player information out of telemetry.
- Local Docker Compose does not replace cloud infrastructure as code.

## Session startup
1. Read this file.
2. Read docs/DEVELOPMENT_STATE.md and docs/ROADMAP.md.
3. Read docs/ARCHITECTURE.md, decisions, and relevant module documents.
4. Inspect Git branch, status, recent history, and available remote state.
5. Report completed work, current task, issues, and recommended action.
6. Await task approval before implementation. Documentation updates are
   automatically authorized; non-documentation changes require explicit
   authorization.

## Task format
Provide the task ID, objective, prerequisites, concept explanation,
file-level instructions, walkthrough, relevant snippets, validation,
acceptance criteria, documentation changes, and Git guidance.

## Session completion
- Review actual changes against acceptance criteria.
- Report tests run, results, and unverified behavior.
- Update documentation automatically to reflect verified work, outstanding
  issues, decisions, and the next action. Summarize the edits for the owner.
- Record unfinished work and the next recommended task.
- Do not finalize a milestone until its documentation is committed.
- Do not claim cross-computer continuity until relevant changes are
  committed, pushed, and synchronization is verified.
- Never overwrite uncommitted work.
- Do not store secrets in Git.

## Documentation
For GitHub task tracking, read docs/WORK_TRACKING.md and the active issue.
Keep stable roadmap IDs and record actual GitHub issue numbers/URLs.
Reference issues in owner-created commits and PRs. Use closing keywords
only when acceptance criteria are satisfied; partial scaffolds do not
complete TECH-001. Verify remote tracking changes before claiming success.
Issue/Project mutations require user authorization; automatic documentation
permission alone does not authorize writes to external services.

The repository is the durable source of project context.
DEVELOPMENT_STATE.md records current facts.
ROADMAP.md records tasks, dependencies, and acceptance criteria.
ARCHITECTURE.md records the approved design and open questions.
DECISIONS.md records significant decisions and their status.
Additional setup, data model, API, testing, and backlog documents may be
created and updated automatically as needed for the current work.
Documentation must not present proposals as approved decisions or planned
functionality as implemented or verified.
