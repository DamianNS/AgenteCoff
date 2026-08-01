Build, test, and lint

- Restore dependencies:
  - dotnet restore

- Build (project-specific):
  - dotnet build ./AgenteCoff.Web/AgenteCoff.Web.csproj -c Release
  - dotnet build ./AgenteCoff.ApiService/AgenteCoff.ApiService.csproj -c Release

- Run locally (development configuration):
  - dotnet run --project AgenteCoff.Web
  - dotnet run --project AgenteCoff.ApiService

- Tests:
  - Run all tests: dotnet test ./AgenteCoff.Tests/AgenteCoff.Tests.csproj
  - Run a single test (examples):
    - By fully-qualified name: dotnet test ./AgenteCoff.Tests/AgenteCoff.Tests.csproj --filter "FullyQualifiedName=MyNamespace.MyTests.MyTestMethod"
    - By test name: dotnet test ./AgenteCoff.Tests/AgenteCoff.Tests.csproj --filter "Name=MyTestMethod"
  - Notes: Tests use MSTest (EnableMSTestRunner) and Aspire.Hosting.Testing helpers.

- Publish / containers (release flow):
  - Regenerate Docker compose and .env (requires aspire CLI):
    - aspire publish -p docker-compose -o ./publicacion
  - Build container images (dotnet Publish to container target):
    - dotnet publish .\AgenteCoff.ApiService\AgenteCoff.ApiService.csproj -c Release /t:PublishContainer -p:ContainerImageName=agentecoff-api
    - dotnet publish .\AgenteCoff.Web\AgenteCoff.Web.csproj -c Release /t:PublishContainer -p:ContainerImageName=agentecoff-web
  - Start containers (uses Podman in docs):
    - cd publicacion && podman compose down && podman compose up -d

- Lint/format:
  - No repository-wide linter or editorconfig detected. Optionally use dotnet format or add Roslyn analyzers if desired: dotnet tool install -g dotnet-format && dotnet format

High-level architecture

- Multi-project .NET 10 (net10.0) repository using Aspire hosting primitives.
- Main projects of interest:
  - AgenteCoff.Web — ASP.NET Core web frontend (net10.0).
  - AgenteCoff.ApiService — backend API; uses Entity Framework Core with SQLite.
  - AgenteCoff.AppHost — application host wiring and container hosting integration (Aspire.Hosting.Docker present).
  - AgenteCoff.ServiceDefaults — shared DI/service defaults, telemetry, resilience helpers.
  - AgenteCoff.Tests — MSTest-based test project using Aspire.Hosting.Testing for integration-style tests.
  - publicacion — generated docker-compose.yaml, .env and deployment artifacts produced by aspire publish.

Key conventions and repo-specific patterns

- Aspire framework and tooling
  - Docker/compose assets and local dashboard are produced by the aspire CLI (see docs/comandos.txt). The project expects Aspire.Hosting packages and testing helpers.

- Container publishing
  - Uses dotnet publish with /t:PublishContainer and ContainerImageName property. Image names in scripts: agentecoff-api, agentecoff-web.

- Tests
  - Tests use MSTest (EnableMSTestRunner in csproj) and may use Aspire.Hosting.Testing helpers to spin up host fixtures. Prefer running tests with dotnet test from the tests project path.

- Configuration
  - appsettings.* JSON files live inside project folders (e.g., AgenteCoff.Web\appsettings.Development.json). Local dev configuration often uses the generated publicacion/.env and compose files.

- No repository-level CI workflows found (no .github/workflows). Local tooling in docs/comandos.txt is the canonical place for publish/redeploy steps.

Files to read first

- docs/comandos.txt — scripted publish/deploy recipe used by maintainers.
- AgenteCoff.Web/AgenteCoff.Web.csproj and AgenteCoff.ApiService/*.csproj — quick view of target frameworks, package references.
- AgenteCoff.Tests/AgenteCoff.Tests.csproj — shows how tests are wired (MSTest + Aspire.Hosting.Testing).

AI assistant notes for future Copilot sessions

- Prefer dotnet CLI commands scoped to the project (explicit project paths) rather than running at repository root (this repo lacks a top-level .sln file).
- When investigating runtime behavior, check publicacion/docker-compose.yaml and the aspire-generated .env before running services.
- Integration tests may depend on Aspire test helpers; run tests from the tests project directory to avoid runner misconfiguration.

MCP servers

- This repository contains an ASP.NET Core web project; consider configuring a Playwright MCP server for end-to-end web testing if browser E2E coverage is desired. Ask if Playwright or a similar MCP server should be added.

Summary

Created concise Copilot instructions covering build, test, and publish commands, high-level architecture, and repository-specific conventions. If any area needs more detail (examples for common test classes, preferred dotnet SDK version pinning, or adding CI workflows), say which section to expand and it will be updated.