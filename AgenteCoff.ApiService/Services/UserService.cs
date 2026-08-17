using Microsoft.AspNetCore.Identity;

namespace AgenteCoff.ApiService.Services
{
    public class UserService(UserManager<IdentityUser> usrMgr, IHttpContextAccessor httpContextAccessor)
    {
        public async Task<IdentityUser?> GetUserByEmailAsync(string email)
        {
            return await usrMgr.FindByEmailAsync(email);
        }

        public async Task<IdentityUser?> GetUser()
        {           
            var httpContext = httpContextAccessor.HttpContext;
            if (httpContext?.User?.Identity?.IsAuthenticated == true)
            {
                var email = httpContext.User.Identity.Name;

                var user = await usrMgr.GetUserAsync(httpContext?.User);

                return await usrMgr.FindByEmailAsync(email);

                
            }
            return null;
        }
    }
}
