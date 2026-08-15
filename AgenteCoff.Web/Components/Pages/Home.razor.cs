using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;

namespace AgenteCoff.Web.Components.Pages;

public partial class Home : ComponentBase
{
    [Inject]
    public IHttpContextAccessor HttpContextAccessor { get; set; } = null!;

    private bool _isAuthenticated;
    private string? _userName;

    [CascadingParameter]
    public Task<AuthenticationState>? AuthStateTask { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
            return;

        var user = HttpContextAccessor.HttpContext?.User;
        _isAuthenticated = user?.Identity?.IsAuthenticated ?? false;
        _userName = _isAuthenticated ? user?.Identity?.Name : null;
        StateHasChanged();
    }

    public bool IsAuthenticated => _isAuthenticated;
    public string? UserName => _userName;
}