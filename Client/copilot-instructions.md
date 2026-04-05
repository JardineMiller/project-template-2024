# Client — Copilot Instructions

Purpose

- Build, test, and run frontend workflows; provide quick commands and required standards.

Stack

- Vue 3 with PrimeVue components. TypeScript is used across the client.

Prereqs

- Node.js >= 18

Common commands

```
cd Client
npm ci
npm run dev
npm run build
npm run test:unit
```

Coding & formatting standards

- JSDoc: Add JSDoc comments for exported functions, classes, and utilities.
- Linting & formatting: Use ESLint and Prettier. The agent runs `Run ESLint` and `Run Prettier` skills before opening PRs.
- Do not commit `.env.development` (use it for local env only). Provide an `env.d.ts` file that defines the expected environment variables types.

Testing

- Unit tests (Vitest) are required for new features. Run `npm run test:unit` locally before opening a PR.
