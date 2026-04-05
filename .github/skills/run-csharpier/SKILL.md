---
name: run-csharpier
description: Format C# source files using CSharpier before committing changes.
---

Trigger: When staged or modified C# files are present.

Steps:

1. Ensure the CSharpier tool is available:
   - If using a tool manifest: `cd Backend && dotnet tool restore`
   - Or install globally: `dotnet tool install -g csharpier` (one-time step for the machine)
2. Navigate to the Backend folder:
   - `cd Backend`
3. Run a verification (no-write) check:
   - If the tool is available via `dotnet tool run`: `dotnet tool run csharpier -- --check .`
   - Or if `csharpier` is globally installed: `csharpier . --check`
4. To apply formatting changes (if check fails):
   - `dotnet tool run csharpier -- .` or `csharpier .`
5. If formatting modified files, stage the changes and re-run the checks. Do not commit until checks pass.
6. If the formatter cannot fix an issue, surface the error in the PR and request human review.

Notes:

- Prefer using a tool manifest (`dotnet-tools.json`) and `dotnet tool restore` for reproducible CI behavior.
- The agent should fail fast: block commits when formatting violations remain.
