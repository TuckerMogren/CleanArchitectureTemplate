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

## Use As a `dotnet new` Template

Install locally from this folder and create a new service:

```bash
# From the repo root
dotnet new install .

# Create a new service named MyService (API only)
dotnet new clean-arch-service -n MyService

# Or include a Worker microservice as well
dotnet new clean-arch-service -n MyService --includeWorker true
```

Notes:
- The solution name `CleanArchitectureTemplate.sln` is replaced with your `-n`.
- Pass `--includeWorker true` to add `src/Worker` and its solution entry.
- After generation, run `dotnet restore` and open the `.sln`.

## Notes

- Keep domain pure. No EF Core or external packages in Domain.
- Application references Domain only. No EF Core or ASP.NET here.
- Infrastructure references Application and Domain. Provide implementations for abstractions.
- WebApi references Application and Infrastructure.
