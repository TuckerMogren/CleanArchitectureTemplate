# Clean Architecture Template (.NET 8)

Structure:
- `src/Domain` — Enterprise entities, value objects, domain events, interfaces.
- `src/Application` — Use cases, commands, queries, validators, abstractions.
- `src/Infrastructure` — Cross-cutting services (e.g., email, time, logging adapters).
- `src/Infrastructure.Persistence` — EF Core persistence, repositories, migrations.
- `src/WebApi` — API endpoints, DI composition root.
- `src/Tests/ArchitectureTests` — NetArchTest rules to enforce layering.
- `src/Tests/UnitTests` — Unit test project scaffold.

## Build

```bash
dotnet restore
dotnet build
dotnet test src/Tests/ArchitectureTests/ArchitectureTests.csproj
```

## Notes

- Keep domain pure. No EF Core or external packages in Domain.
- Application references Domain only. No EF Core or ASP.NET here.
- Infrastructure references Application and Domain. Provide implementations for abstractions.
- WebApi references Application and Infrastructure.