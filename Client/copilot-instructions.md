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

### Vitest + PrimeVue setup

When writing Vitest tests for Vue components that use PrimeVue:

- Always install the PrimeVue plugin in the test's `global.plugins`.
- Register PrimeVue components in `global.components` as in the main app (e.g., InputText, Password, Button, Message, etc.).
- Do NOT stub PrimeVue components unless you have a specific reason (e.g., isolating a visual or async behavior).
- If a component requires a plugin (like GoogleLogin), mock only that plugin/component as needed, but use the real PrimeVue setup.

Example:

```ts
import InputText from "primevue/inputtext";
import Password from "primevue/password";
import Message from "primevue/message";
import PrimeVue from "primevue/config";
import Button from "primevue/button";

wrapper = mount(MyComponent, {
    global: {
        plugins: [PrimeVue],
        components: { InputText, Password, Button, Message },
    },
});
```

This ensures tests run against the real component tree and avoid $primevue injection errors.
