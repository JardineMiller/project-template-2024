---
name: run-prettier
description: Check and optionally fix frontend formatting using Prettier.
---

Trigger: When staged or modified frontend files (JS/TS/Vue/CSS) require formatting checks.

Steps:

1. From the repository root or `Client/` folder run a check:
   - `npx prettier . --check` (or `npx prettier "src/**/*.{js,ts,vue,css}" --check` to narrow scope)
2. If there are formatting fixes to apply, do not ask for permission, just run:
   - `npx prettier . --write` (or narrow to staged files)
3. Stage any modified files and re-run the check. Do not commit until Prettier checks pass.

Notes:

- Prefer running Prettier on a narrow set of staged files in CI hooks to reduce runtime.
- The agent should not modify `.prettierrc` or other formatting configs without human approval.
