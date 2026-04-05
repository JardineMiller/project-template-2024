## Summary

Remove Mapster and replace with explicit mapping extension methods. Mappings have been split into domain-specific files and controllers/tests updated to use the explicit mappings. Formatting and tests were run.

## Changes

- Removed Mapster package references from `Backend/ProjectTemplate2024.Api/ProjectTemplate2024.Api.csproj`.
- Deleted Mapster mapping config files and DI registration under `Backend/ProjectTemplate2024.Api/Common/Mapping/`.
- Added explicit mapping extension files:
  - `Backend/ProjectTemplate2024.Api/Common/Mapping/AuthenticationMapping.cs`
  - `Backend/ProjectTemplate2024.Api/Common/Mapping/UserMapping.cs`
  - `Backend/ProjectTemplate2024.Api/Common/Mapping/AccountMapping.cs`
  - `Backend/ProjectTemplate2024.Api/Common/Mapping/MappingHelpers.cs`
- Updated controllers to use the new mapping methods:
  - `Backend/ProjectTemplate2024.Api/Controllers/AuthController.cs`
  - `Backend/ProjectTemplate2024.Api/Controllers/UsersController.cs`
  - `Backend/ProjectTemplate2024.Api/Controllers/AccountController.cs`
- Updated unit tests under `Backend/ProjectTemplate2024.Tests/Api.Tests/Common/Mapping` to assert the explicit mappings.
- Ran CSharpier and applied formatting changes.

## How to test

Run the commands below locally:

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

## Checklist

- [x] `dotnet build` passes
- [x] `dotnet test` passes
- [ ] `npm run test:unit` passes
- [x] No secrets included
