# Backend — Copilot Instructions

Purpose

- Build, test, and run backend workflows. Provide exact commands and safe defaults for local development.

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
