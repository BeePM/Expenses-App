using Expenses.API.Interfaces;

namespace Expenses.API.Services
{
    public class DevelopmentUserService : IUserService
    {
        public string? GetUserId()
        {
            return "local-development";
        }
    }
}
