# Clean Architecture Template (.NET 8)

Opinionated starter for building services with Clean Architecture and optional Worker service. Includes solution template metadata so it can be used via `dotnet new`.

## Structure

- `src/Domain` — Enterprise entities, value objects, domain events, interfaces.
- `src/Application` — Use cases, commands, queries, validators, abstractions.
- `src/Infrastructure` — Cross-cutting services (e.g., email, time, logging adapters).
- `src/Infrastructure.Persistence` — Persistence layer placeholder (add EF Core or other provider here).
- `src/WebApi` — API host, DI composition root, Swagger in Development.
- `src/Worker` — Optional background worker host (included via template option).
- `src/Tests/ArchitectureTests` — NetArchTest rules to enforce layering.
- `src/Tests/UnitTests` — Unit test project scaffold (xUnit + FluentAssertions + Moq).

### Project References (allowed dependencies)

- `Application` → `Domain`
- `Infrastructure` → `Application`, `Domain`
- `Infrastructure.Persistence` → (isolated; no direct refs to other layers by default) or → `Infrastructure`,
- `WebApi` → `Application`, `Infrastructure`, `Infrastructure.Persistence`
- `Worker` → `Application`, `Infrastructure`, `Infrastructure.Persistence`

Architecture tests in `src/Tests/ArchitectureTests` validate these rules and ensure `Application` does not reference `Microsoft.EntityFrameworkCore`.

## Prerequisites

- **.NET SDK**: Install .NET 8 SDK (`dotnet --version` should report 8.x).
- **CLI access**: Commands assume running from a shell at the repo root.
 - **SDK pin**: `global.json` targets `8.0.100`. Install that SDK or adjust the file.

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

### Template Parameters

- `--includeWorker` (bool, default `false`): include the `Worker` project.

The template replaces all occurrences of `CleanArchitectureTemplate` with the name you provide via `-n`.

### Common CLI Tasks (Solution Template)

- **List installed template**: `dotnet new list clean-arch-service`
- **Uninstall template**: `dotnet new uninstall .`
- **Reinstall/update from this repo**: `dotnet new install .`
- **Generate into current folder**: `dotnet new clean-arch-service -n MyService -o .`
- **Boolean option**: `--includeWorker` can be passed as `--includeWorker` or `--includeWorker true`.

### Run After Scaffolding

- **Restore**: `dotnet restore`
- **Build**: `dotnet build`
- **Run API**: `dotnet run --project src/WebApi/WebApi.csproj`
  - Swagger is enabled in Development. This starter does not include endpoints by default; add minimal APIs or controllers to see operations in Swagger.
- **Run Worker (if included)**: `dotnet run --project src/Worker/Worker.csproj`
  - Worker is scaffolded as an empty host. Add a `BackgroundService` or hosted service to start processing work.
- **Run architecture tests**: `dotnet test src/Tests/ArchitectureTests/ArchitectureTests.csproj`

### Troubleshooting

- **Template not showing**: Rebuild template cache with `dotnet new --debug:reinit`, then `dotnet new install .`.
- **Wrong name replacements**: Ensure you used `-n <Name>`; the template replaces `CleanArchitectureTemplate` with your name across files and the `.sln`.
- **SDK mismatch**: Verify `global.json` SDK is installed or remove/adjust `global.json`.

## Adding Persistence (EF Core example)

This starter keeps Domain and Application free of EF Core. Add EF Core to `Infrastructure.Persistence` and wire it from `WebApi`/`Worker`.

```bash
# Add EF Core provider and tools to Infrastructure.Persistence
dotnet add src/Infrastructure.Persistence/Infrastructure.Persistence.csproj package Microsoft.EntityFrameworkCore
dotnet add src/Infrastructure.Persistence/Infrastructure.Persistence.csproj package Microsoft.EntityFrameworkCore.SqlServer
dotnet add src/Infrastructure.Persistence/Infrastructure.Persistence.csproj package Microsoft.EntityFrameworkCore.Design

# (Optional) add tools at solution level for migrations
dotnet tool install --global dotnet-ef

# Create an initial migration (example)
dotnet ef migrations add Initial --project src/Infrastructure.Persistence --startup-project src/WebApi

# Apply migrations
dotnet ef database update --project src/Infrastructure.Persistence --startup-project src/WebApi
```

Remember: keep `DbContext` and mappings in `Infrastructure.Persistence`. Inject abstractions into `Application` if needed; do not reference EF types from `Application`.

## Code Style & Build Settings

- `.editorconfig`: formatting and style preferences (CRLF, 4 spaces, C# style hints).
- `Directory.Build.props`: shared build settings (nullable on, implicit usings, preview language, XML docs).

## Contributing

- Follow the layering rules; run `ArchitectureTests` before PRs.
- Add unit tests under `src/Tests/UnitTests` for new behavior.
- Keep `Domain` pure (no external packages), and avoid EF Core in `Application`.

## Notes

- Keep domain pure. No EF Core or external packages in Domain.
- Application references Domain only. No EF Core or ASP.NET here.
- Infrastructure references Application and Domain. Provide implementations for abstractions.
- WebApi references Application and Infrastructure.

---

Template metadata: see `.template.config/template.json` (`shortName`: `clean-arch-service`, option: `includeWorker`).


Note: Parts of this readme, and template, where generated via AI; Verified by a human author. 