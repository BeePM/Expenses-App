using Expenses.Common;
using Expenses.Common.DTO;

namespace Expenses.MAUI.Interfaces;

public interface IExpenseService
{
    Task<IEnumerable<ExpenseDTO>?> GetExpensesAsync(DateOnly? startDate = null,
                                                   DateOnly? endDate = null,
                                                   string? category = null,
                                                   int? offset = 0,
                                                   int? limit = Constants.DefaultPageSize);
}
