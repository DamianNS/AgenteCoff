using AgenteCoff.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AgenteCoff.Web.Controllers
{
    [ApiController]
    public class LocalAuthController : ControllerBase
    {
        private readonly ApiClient authApiClient;
        private readonly HttpContext httpContext;

        public LocalAuthController(ApiClient authApiClient, IHttpContextAccessor _httpContextAccessor)
        {
            this.authApiClient = authApiClient;
            this.httpContext = _httpContextAccessor.HttpContext;            
        }

        [HttpGet("/auth/logout")] // O [HttpPost] según prefieras
        public async Task<IActionResult> LogoutAsync()
        {
            // Esto elimina la cookie de autenticación del navegador de forma segura
            await HttpContext.SignOutAsync("login");

            // Redirige al usuario de vuelta a la página de inicio o login de Blazor
            return Redirect("/");
        }

        [HttpPost("/auth/login")]
        public async Task<IActionResult> Login([FromForm] LoginRequest request)
        {
            // 1. Llamar a tu API backend que te regresa el JWT
            var response = await authApiClient.LoginAsync(request);

           if(response != null)
            {

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, response.UserName),
                    new Claim("ElTocken", response.Token)
                };
                var claimsIdentity = new ClaimsIdentity(claims, "login");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await httpContext.SignInAsync("login", claimsPrincipal);

                //httpContext.Response.Cookies.Append("X-Access-Token", response.Token, new CookieOptions
                //{
                //    HttpOnly = true,
                //    Secure = true,
                //    SameSite = SameSiteMode.Strict,
                //    Expires = DateTimeOffset.UtcNow.AddHours(1) // Ajusta según la expiración de tu JWT
                //});

                return Redirect("/");
            }

            // 2. Guardar el JWT en una cookie HttpOnly y Segura
            return Forbid();
        }
    }
}
