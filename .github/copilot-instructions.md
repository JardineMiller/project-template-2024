# Copilot Agent — Repository Instructions

Purpose

- Onboard GitHub Copilot (agent mode) to work safely and deterministically across this repository.

Repo summary

- Backend: C#, ASP.NET Core Web API, EF Core, .NET 10
- Client: Vue 3 + Vite, TypeScript, Vitest (unit)

Prerequisites

- Install .NET 10 SDK
- Install Node.js (>= 18)

Allowed agent actions

- Create branches, run builds/tests locally, and open PRs for human review.
- Modify code and tests, add docs, and update scaffolding.

Forbidden actions

- Commit secrets or credentials into repository files.
- Modify GitHub repository secrets or merge to protected branches without explicit human approval.

Quick commands

```
# Backend
cd Backend
dotnet restore
dotnet build
dotnet test

# Client
cd Client
npm ci
npm run test:unit
```

Mapping

- Backend workflows: `Backend/.agent.md`, `Backend/copilot-instructions.md`
- Client workflows: `Client/.agent.md`, `Client/copilot-instructions.md`
- PR & policy: `.agent.md`, `.prompt.md`, `.github/SKILL.md`

Contact & escalation

- Agent must open PRs for human review; tag maintainers listed in CONTRIBUTING.md (or repo owners).

Project Architecture & Stack

- Backend: .NET 10 (Clean Architecture), SQL Server. Follow feature-by-folder organization (group domain, application, persistence and tests around features).
- Frontend: Vue 3 with PrimeVue components.

Dependencies

- Never change project inter-dependencies without human approval. Do not add or remove project references across solution projects without a maintainer review.

Coding & Documentation Standards

- C#: Mandatory XML documentation comments for public methods and classes. Use CSharpier to format C# files.
- JS/TS: Mandatory JSDoc for exported methods, classes, and utilities. Use ESLint and Prettier for linting and formatting.
- Features: For every new feature, produce a short technical Markdown document describing the design, API contract, and any migrations.
- Code style: Prefer simple, self-documenting code.

Pre-commit & CI checks

- Agent must run formatting and linting checks before committing: run the `Run CSharpier` skill for C# files and `Run ESLint` + `Run Prettier` skills for frontend files.
- Unit tests are mandatory: backend unit tests (`dotnet test`) and frontend Vitest (`npm run test:unit`) must pass locally before opening a PR.

Testing & DevOps

- Required: Unit tests for backend and Vitest for frontend are non-negotiable and must be included with new features.
- Excluded: Playwright E2E tests are explicitly out-of-scope for agent workflows at this time.

Security & Secrets

- Never commit connection strings or secrets into repository files. For backend use `dotnet user-secrets`; for frontend use `.env.development` and an `env.d.ts` typing file (keep `.env.development` out of VCS).
- Agent must not write secrets into the repo or add sensitive data to commits.

Skills and automation

- Skill definitions live under `.github/<skill-name>/SKILL.md`. Agents should invoke the corresponding skill before taking actions that affect code formatting, linting, or DB migrations.
