using Expenses.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace Expenses.API.Database
{
    public interface IExpensesContext
    {
        DbSet<Expense> Expenses { get; }
        DbSet<ExpenseCategory> ExpensesCategories { get; }

        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
