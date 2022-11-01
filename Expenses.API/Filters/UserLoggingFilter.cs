using Expenses.API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Expenses.API.Filters
{
    public class UserLoggingFilter : IActionFilter
    {
        private readonly ILogger<UserLoggingFilter> _logger;
        private readonly IUserService _userService;

        public UserLoggingFilter(ILogger<UserLoggingFilter> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        { }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = _userService.GetUserId();
            _logger.LogInformation("User '{UserId}' requesting resource -> '{Resource}'",
                                   userId,
                                   context.ActionDescriptor.DisplayName ?? context.ActionDescriptor.Id);
        }
    }
}
