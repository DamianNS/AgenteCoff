using AgenteCoff.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace AgenteCoff.Web.Components.Pages;

public partial class Weather : ComponentBase
{
    [CascadingParameter]
    private Task<AuthenticationState>? AuthState { get; set; }

    [Inject]
    private ApiClient WeatherApi {  get; set; }

    public bool IsAuthenticated;
    public IEnumerable<Claim> claims => AuthState?.Result?.User?.Claims ?? Enumerable.Empty<Claim>();
    public WeatherForecast[]? Forecasts { get; set; }

    public string? userName { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var a  = await AuthState;
        IsAuthenticated = a?.User?.Identity?.IsAuthenticated ?? false;
        userName = a?.User?.Identity?.Name;

        if (AuthState == null)
        {
            return;
        }

        try
        {
            Forecasts = await WeatherApi.GetWeatherAsync();
        }
        catch (UnauthorizedAccessException)
        {
            // Handle unauthorized access if necessary
        }
    }
}