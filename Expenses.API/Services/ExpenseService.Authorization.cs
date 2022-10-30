using Expenses.API.Exceptions;
using Expenses.Common.Entities;

namespace Expenses.API.Services
{
    public partial class ExpenseService
    {
        private void AuthorizeExpenseOperation(Expense expense)
        {
            var currentUserId = _userService.GetUserId();
            if (currentUserId != expense.UserId)
            {
                throw new AuthorizationException();
            }
        }
    }
}
