using System.Net.Http.Headers;
using System.Linq;

namespace AgenteCoff.Web.Providers
{
    public class CookieToJwtHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieToJwtHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // 1. Obtener el HttpContext de la petición actual del usuario en Blazor SSR
            var httpContext = _httpContextAccessor.HttpContext;

            // 2. Buscar la cookie donde guardamos el JWT
            if (httpContext?.User?.Identity?.IsAuthenticated ?? false)
            {
                var claim = httpContext?.User?.Claims.ToList().FirstOrDefault(c=> c.Type == "ElTocken");

                if (claim != null && !string.IsNullOrEmpty(claim.Value))
                {
                    // 3. Adjuntar el token como Bearer en la cabecera de la llamada al Backend
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", claim.Value);
                }
            }

            // 4. Continuar con la petición HTTP
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
