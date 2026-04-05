---
name: ef-db-migration
description: Create and apply EF Core migrations and verify persistence for schema changes.
---

Trigger: When a domain entity or persistence shape is added or changed.

Steps:

1. Add or update the entity within the Domain project (follow feature-by-folder layout):
   - Edit or add files under `Backend/ProjectTemplate2024.Domain/Entities/<Feature>/`.
2. Create a migration (from repository root):
   - Ensure EF tools: `dotnet tool restore` (if using tool manifest) or install `dotnet-ef` globally.
   - Add migration:
     `dotnet ef migrations add <MigrationName> -p ProjectTemplate2024.Infrastructure -s ProjectTemplate2024.Api`
3. Apply the migration to the target database:
   - `dotnet ef database update -p ProjectTemplate2024.Infrastructure -s ProjectTemplate2024.Api`
   - For CI or disposable DBs, prefer starting a containerized SQL Server and point `ConnectionStrings__DefaultConnection` to it.
4. Run and test the application to verify persistence:
   - `cd Backend/ProjectTemplate2024.Api`
   - `dotnet run`
   - Run relevant integration tests or exercise the API endpoints that touch the updated entities.
5. Document the migration and include the generated migration files in the PR. Do not commit any connection strings; use `dotnet user-secrets` for local credentials.

Notes:

- If a migration affects production-critical tables, request human review and a maintenance window before applying in production.
- Always include a short Markdown note describing the migration and any necessary rollback steps.
