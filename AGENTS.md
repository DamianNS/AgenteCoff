# 🤖 AgenteCoff Quickstart Guide

## 🏗️ Architecture Overview
- **webfrontend (Port 8080):** UI interface. Communicates with `apiservice` over `aspire` network.
- **apiservice (Port 5001):** Core business logic. Uses `agentecoff.db` (SQLite).
- **Dashboard (Port 18889/18888):** OTLP/gRPC metrics endpoint.

## 📡 Communication (Telemetry)
- Agents export diagnostics via **OTLP/gRPC** to the Dashboard.
- **Endpoint:** `http://mi-red-hogarena-dashboard:18889`
- **Protocol:** `grpc`
- **Auth:** Requires `DASHBOARD__OTLP__AUTHMODE: ApiKey`

## ⚙️ Workflow & Commands
- **Build/Deploy:** CI is handled by `.github/workflows/deploy.yml`.
    - Updates involve `docker compose pull` via SSH (port `5583`) to update containers.
- **Local Debugging:** Check `agentecoff.db` for data persistence.
- **Troubleshooting Flow:**
    1. Check Nginx Proxy Manager forwarding to `webfrontend:8080`.
    2. Verify API connectivity within the `aspire` network.
    3. Inspect OTLP variables if telemetry is failing.

## 🔍 Investigation Hierarchy
1. **Config/Docs:** `README*`, `opencode.json`, `.opencode/instructions`.
2. **Entrypoints:** Inspect `webfrontend` and `apiservice` codebases to find main execution paths.
3. **Truth Source:** Trust executable scripts/CI workflows over prose documentation for build/deployment steps.