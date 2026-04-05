---
name: run-eslint
description: Lint and optionally fix frontend JavaScript/TypeScript files using ESLint.
---

Trigger: When staged or modified JS/TS files are present in the `Client/` project.

Steps:

1. Navigate to the Client folder:
   - `cd Client`
2. Ensure dependencies are installed (CI: `npm ci`, local dev: `npm install`).
3. Run the lint script:
   - `npm run lint`
4. To auto-fix fixable issues, run:
   - `npm run lint -- --fix`
5. Re-run the lint command and ensure no errors remain. Do not commit until the linter is clean.

Notes:

- The agent should not add, remove, or upgrade packages as part of lint fixes; any dependency changes require human approval.
- If lint rules require configuration changes, open a draft PR describing the proposed rule change and request a reviewer.
