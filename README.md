# AgenteCoff

A .NET 10 Blazor API service for weather forecasting with authentication and webhook support.

## Architecture

- **API Service**: Core business logic on port 5001
- **Web Frontend**: Blazor UI on port 8080
- **Database**: SQLite with EF Core (`Data Source=/app/data/agentecoff.db`)

## Project Structure

```
AgenteCoff.ApiService/
├── Controllers/
│   ├── AuthController.cs     # Login/Registration
│   ├── WeatherController.cs  # Weather data endpoints
│   └── WebhookController.cs  # Webhook handling
├── Data/
│   ├── Models/
│   │   ├── Aviso.cs          # Alert notifications
│   │   ├── WeatherForecastEntity.cs  # Weather forecast data
│   │   └── NotifyDTO.cs     # Webhook payload storage
├── Models/
│   └── AuthModels.cs        # Authentication models
├── Services/
│   └── WeatherService.cs    # Weather logic
└── Program.cs               # Entry point
```

## Authentication & Authorization

- **Authentication**: JWT Bearer with 60-minute expiry
- **JWT Configuration**:
  - Key: `AgenteCoffLocalDevelopmentKey!2026-2027-ValidKey`
  - Issuer: `AgenteCoff`
  - Audience: `AgenteCoff-Users`
- **Default Admin Account** (created on first run):
  - Email: `admin@agentecoff.local`
  - Password: `Password123!`

## APIs

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/api/Weather` | GET | Fetch weather forecasts (5-day sample) |
| `/api/Weather/forecast` | GET | Get weather forecast by ID |
| `/api/Auth/login` | POST | User login |
| `/api/Auth/register` | POST | User registration |
| `/api/Webhook` | POST | Receive external webhook payloads |

## Webhook Interface

```csharp
public class WebhookPayload
{
    public string? PackageName { get; set; }
    public string? AppName { get; set; }
    public string? Title { get; set; }
    public string? Text { get; set; }
}
```

## Project Setup

### Build
```bash
dotnet build
```

### Run
```bash
dotnet run
```

### Database
- **Location**: `D:\src\AgenteCoff\AgenteCoff.ApiService\agentecoff.db`
- **Verify**: Check file exists and is accessible

## License

AgenteCoff
