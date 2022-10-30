using Expenses.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace Expenses.API.Database
{
    public class ExpensesContext : DbContext
    {
        public ExpensesContext(DbContextOptions<ExpensesContext> options) : base(options)
        { }

        public DbSet<Expense> Expenses => Set<Expense>();
        public DbSet<ExpenseCategory> ExpensesCategories => Set<ExpenseCategory>();
    }
}
