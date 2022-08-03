using System.Security.Claims;

namespace Backend
{
    public class CurrentUser
    {
        private readonly HttpContextAccessor _httpContextAccessor;

        public CurrentUser(HttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUsername()
        {
            return _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        }
    }
}
