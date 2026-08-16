using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace AgenteCoff.Web.Components.Pages;

public partial class Login : ComponentBase
{
    public bool isAuthenticated;

    public string? userName;

    private LoginFormModel loginModel = new();

    [CascadingParameter]
    private Task<AuthenticationState>? AuthStateTask { get; set; }
    [Inject]
    public NavigationManager Navigation { get; private set; }

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            isAuthenticated = AuthStateTask?.Result.User.Identity?.IsAuthenticated ?? false;
            if (isAuthenticated)
            {
                userName = AuthStateTask?.Result.User.Identity?.Name;
            }
            StateHasChanged();
        }
        return base.OnAfterRenderAsync(firstRender);
    }

    private async Task LogoutAsync()
    {
        Navigation.NavigateTo("/auth/logout", forceLoad: true);
    }

    private class LoginFormModel
    {
        public string Email { get; set; } = "admin@agentecoff.local";
        public string Password { get; set; } = "Password123!";
    }


}