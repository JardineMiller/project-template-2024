<!-- PR template filled by agent when opening PRs -->

## Summary

One-paragraph description of the change.

## Changes

- Files added/modified (brief list)

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

- [ ] `dotnet build` passes
- [ ] `dotnet test` passes
- [ ] `npm run test:unit` passes
- [ ] No secrets included
