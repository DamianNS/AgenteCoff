using System.Net;

namespace AgenteCoff.Web.Services;

public class ApiClient(HttpClient httpClient)
{   

    public async Task<AuthResult?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("/api/auth/login", request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<AuthResult>(cancellationToken: cancellationToken);
    }


    public async Task<WeatherForecast[]> GetWeatherAsync(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        
        using var request = new HttpRequestMessage(HttpMethod.Get, "/weatherforecast");

        using var response = await httpClient.SendAsync(request, cancellationToken);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException("You must sign in to access the weather forecast.");
        }

        response.EnsureSuccessStatusCode();

        var forecasts = await response.Content.ReadFromJsonAsync<WeatherForecast[]>(cancellationToken: cancellationToken) ?? [];
        return forecasts.Take(maxItems).ToArray();
    }
}

public record AuthResult(string Token, string UserName, DateTime ExpiresAt);

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
