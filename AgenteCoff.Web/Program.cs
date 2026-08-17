using AgenteCoff.Web.Components;
using AgenteCoff.Web.Providers;
using AgenteCoff.Web.Services;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);



builder.AddServiceDefaults();

builder.Services.AddControllers();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOutputCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDataProtection();
builder.Services.AddTransient<CookieToJwtHandler>();
builder.Services.AddHttpClient<ApiClient>(client => client.BaseAddress = new("http://apiservice"))
    .AddHttpMessageHandler<CookieToJwtHandler>();
builder.Services.AddScoped<ProtectedSessionStorage>();

builder.Services.AddScoped<CharacterService>();

builder.Services.AddCascadingAuthenticationState(); // Requerido en Blazor

builder.Services.AddAuthentication("login")
    .AddCookie("login", options =>
    {
        options.Cookie.Name = "X-Access-Token";
    });

//builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();
app.UseOutputCache();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Map API/controllers used by the web frontend (e.g. LocalAuthController)
app.MapControllers();

app.MapDefaultEndpoints();

app.Run();

public record LoginRequest(string Email, string Password);
public record AuthResult(string Token, string UserName, DateTime ExpiresAt);