using Expenses.API.Models;
using Expenses.API.Queries;
using Expenses.Common.Entities;

namespace Expenses.API.Interfaces
{
    public interface IExpenseService
    {
        #region Expense

        Task<List<Expense>> GetAllExpensesAsync(GetAllExpensesQuery query, CancellationToken ct = default);
        Task<Expense> GetExpenseAsync(int expenseId, CancellationToken ct = default);
        Task<Expense> CreateExpenseAsync(CreateExpenseModel model, int categoryId, CancellationToken ct = default);
        Task<Expense> UpdateExpenseAsync(UpdateExpenseModel model, int categoryId, CancellationToken ct = default);
        Task DeleteExpenseAsync(int expenseId, CancellationToken ct = default);

        #endregion

        #region Category

        Task<List<ExpenseCategory>> GetExpenseCategoriesAsync(CancellationToken ct = default);
        Task<ExpenseCategory?> FindExpenseCategoryAsync(string category, CancellationToken ct = default);
        Task<ExpenseCategory> CreateExpenseCategoryAsync(string category, CancellationToken ct = default);
        Task DeleteExpenseCategoryAsync(int categoryId, CancellationToken ct = default);

        #endregion
    }
}
