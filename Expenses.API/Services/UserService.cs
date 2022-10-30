using Expenses.API.Interfaces;

namespace Expenses.API.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string? GetUserId()
        {
            var context = _contextAccessor.HttpContext;
            if (context != null && context.User.Identity?.IsAuthenticated == true)
            {
                return context.User.Identity.Name;
            }

            return "default";
        }
    }
}
