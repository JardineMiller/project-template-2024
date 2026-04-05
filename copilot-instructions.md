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
