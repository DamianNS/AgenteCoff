# AGENTS.md - AgenteCoff Project

## Architecture Overview

- **webfrontend (Port 8080) (AgenteCoff.Web):** Blazor UI. Communicates with `apiservice` via `aspire` network.
- **apiservice (Port 5001):** Core business logic. Uses `agentecoff.db` (SQLite).
- **Dashboard (Port 18889/18888):** OTLP/gRPC metrics endpoint. Auth requires `DASHBOARD__OTLP__AUTHMODE: ApiKey`.

## Web Frontend Stack (AgenteCoff.Web)

- **Framework:** .NET 10 Blazor.
- **Component Structure:** Every component Blazor must have three files: `.razor`, `.cs`, and `.css`.
- **Data Model:** Character data is managed by `CharacterService`.

## Workflow & Commands

- **Local Debugging:** Check `agentecoff.db` for data persistence.
- **API Communication:** Use `HttpClient` injected via `ApiClient` for all external service calls.
- **Build/Run:** 
    - Build: `dotnet build`
    - Run: `dotnet run` (for the Web project)
    - Verification: `dotnet test` (if tests are added)
- **Troubleshooting Flow:**
    1. Check Nginx Proxy Manager forwarding to `webfrontend:8080`.
    2. Verify API connectivity within the `aspire` network.
    3. Inspect OTLP variables if telemetry is failing.

## Constraints

- **Source of Truth:** Trust executable scripts (build/CI) over prose documentation.
- **No Secrets:** Never commit keys or secrets; use environment variables or configuration files.