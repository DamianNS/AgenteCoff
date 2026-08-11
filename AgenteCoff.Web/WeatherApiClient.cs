using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace AgenteCoff.Web;

public class WeatherApiClient(HttpClient httpClient, AuthenticationSessionService authService)
{
    public async Task<WeatherForecast[]> GetWeatherAsync(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        var token = await authService.GetTokenAsync();
        using var request = new HttpRequestMessage(HttpMethod.Get, "/weatherforecast");

        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

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

public class AuthApiClient(HttpClient httpClient)
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
}

public class AuthenticationSessionService(AuthApiClient authApiClient)
{
    private string? currentToken;
    private string? currentUserName;

    public async Task<AuthResult?> LoginAsync(string email, string password)
    {
        var result = await authApiClient.LoginAsync(new LoginRequest(email, password));
        if (result is null)
        {
            return null;
        }

        currentToken = result.Token;
        currentUserName = result.UserName;
        return result;
    }

    public Task LogoutAsync()
    {
        currentToken = null;
        currentUserName = null;
        return Task.CompletedTask;
    }

    public Task<string?> GetTokenAsync()
    {
        return Task.FromResult(currentToken);
    }

    public Task<string?> GetUserNameAsync()
    {
        return Task.FromResult(currentUserName);
    }
}

public record LoginRequest(string Email, string Password);

public record AuthResult(string Token, string UserName, DateTime ExpiresAt);

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
