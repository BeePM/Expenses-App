using Expenses.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace Expenses.API.Database
{
    public class ExpensesContext : DbContext, IExpensesContext
    {
        public ExpensesContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Expense> Expenses => Set<Expense>();
        public DbSet<ExpenseCategory> ExpensesCategories => Set<ExpenseCategory>();
    }
}
