# Project Template 2024

This repository provides a modern full-stack project template with a .NET 10 backend (Clean Architecture) and a Vue 3 + Vite frontend. It is designed for rapid development, maintainability, and best practices in both C# and TypeScript ecosystems.

---

## Repository Structure

```
Backend/                  # ASP.NET Core Web API backend (Clean Architecture)
  ProjectTemplate2024.Api/         # API entry point (controllers, startup)
  ProjectTemplate2024.Application/ # Application layer (CQRS, business logic)
  ProjectTemplate2024.Contracts/   # Shared contracts (DTOs, interfaces)
  ProjectTemplate2024.Domain/      # Domain entities and logic
  ProjectTemplate2024.Infrastructure/ # Persistence, services, email, etc.
  ProjectTemplate2024.Tests/       # Unit tests (API, Application, Infrastructure)

Client/                   # Vue 3 frontend (Vite, TypeScript)
  src/                    # Application source code
  public/                 # Static assets
  ...                     # Config, tests, etc.

.github/                  # Agent, workflow, and skill definitions

README.md                 # This file
```

---

## Prerequisites

- .NET 10 SDK
- Node.js (>= 18)

---

## Backend: Build, Run, Test

1. **Install dependencies:**
   ```sh
   cd Backend
   dotnet restore
   ```
2. **Build the solution:**
   ```sh
   dotnet build
   ```
3. **Run the API:**
   ```sh
   dotnet run --project ProjectTemplate2024.Api
   ```
   The API will start on the port configured in `appsettings.json` or `launchSettings.json`.
4. **Run backend unit tests:**
   ```sh
   dotnet test
   ```

---

## Frontend (Client): Build, Run, Test

1. **Install dependencies:**
   ```sh
   cd Client
   npm ci
   ```
2. **Run the development server:**
   ```sh
   npm run dev
   ```
   The app will be available at the local address shown in the terminal (default: http://localhost:5173).
3. **Run frontend unit tests:**
   ```sh
   npm run test:unit
   ```

---

## Coding Standards & CI

- **Backend:**
  - C# code must be formatted with CSharpier.
  - XML documentation is required for public APIs.
  - Unit tests are mandatory for new features.
- **Frontend:**
  - TypeScript/JS code must pass ESLint and Prettier.
  - JSDoc required for exported symbols.
  - Unit tests (Vitest) are required for new features.

---

## Additional Notes

- **Secrets:** Never commit secrets. Use `dotnet user-secrets` for backend and `.env.development` (excluded from VCS) for frontend.
- **Pull Requests:** All changes must go through PRs and pass all tests and formatting checks.
- **Documentation:** For new features, add a short technical Markdown doc describing the design and API contract.

---

For more details, see the `copilot-instructions.md` files in each project folder and `.github/` for agent and workflow automation.
