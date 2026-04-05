# Backend — Copilot Instructions

Purpose

- Build, test, and run backend workflows. Provide exact commands and safe defaults for local development.

Architecture & folder layout

- This repository follows Clean Architecture principles. Keep domain, application, and persistence code separated and organize by feature (feature-by-folder). Place entities and domain logic under `ProjectTemplate2024.Domain`, feature-specific application code under `ProjectTemplate2024.Application`, and persistence under `ProjectTemplate2024.Infrastructure/Persistence`.

Formatting & docs

- Use CSharpier for code formatting. Run the `Run CSharpier` skill prior to commits.
- Include XML documentation comments for public methods and classes (required).

Dependencies

- Never change project inter-dependencies (project references) without explicit maintainer approval.

Prereqs

- .NET 10 SDK
- (Optional) Docker for SQL Server when running integration tests

Common commands

```
cd Backend
dotnet restore
dotnet build
dotnet test
```

EF Core / Migrations

```
dotnet tool install --global dotnet-ef
dotnet ef migrations add <Name> -p ProjectTemplate2024.Infrastructure -s ProjectTemplate2024.Api
dotnet ef database update -p ProjectTemplate2024.Infrastructure -s ProjectTemplate2024.Api
```

Dev DB (Docker)

```
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Password123!" -p 1433:1433 --name pt-sql -d mcr.microsoft.com/mssql/server:2019-latest
```

Secrets & user-secrets

- Use `dotnet user-secrets` to set `JwtSettings:Secret` and mail credentials during local development.

Pre-commit checks

- Before committing, run formatting (`csharpier`) and unit tests locally. If formatting changes are applied, stage the formatted files and re-run checks.
