---
name: repo-operator-guide
description: Document what the Copilot agent may do in this repository and how humans can control agent behavior.
---

Capabilities

- Create branches, run local builds/tests, add code and tests, scaffold features, and open PRs for review.

Restrictions

- Agent must not commit secrets or change GitHub repository secrets.
- Agent must not merge PRs to protected branches without explicit human approval.

Escalation

- If an agent-created PR needs urgent human attention, tag `@repo-owner` or the on-call maintainer.

Available skills

- `Run CSharpier`: Formats C# files using the repository CSharpier configuration. See `.github/run-csharpier/SKILL.md`.
- `Run ESLint`: Lints frontend JS/TS files and optionally applies fixes. See `.github/run-eslint/SKILL.md`.
- `Run Prettier`: Verifies frontend formatting using Prettier. See `.github/run-prettier/SKILL.md`.
- `Database Migration Workflow`: Create, apply, and verify EF Core migrations. See `.github/db-migration/SKILL.md`.

Skills are authoritative step-by-step procedures the agent should follow when performing automated actions. Do not bypass skills for sensitive tasks.
