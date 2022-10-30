using Expenses.API.Exceptions;
using Expenses.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace Expenses.API.Services
{
    public partial class ExpenseService
    {
        public async Task<List<ExpenseCategory>> GetExpenseCategoriesAsync(CancellationToken ct = default)
        {
            return await _context.ExpensesCategories
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<ExpenseCategory?> FindExpenseCategoryAsync(string category, CancellationToken ct = default)
        {
            return await _context.ExpensesCategories.FirstOrDefaultAsync(x => EF.Functions.Like(x.Name!, category), ct);
        }

        public async Task<ExpenseCategory> CreateExpenseCategoryAsync(string category, CancellationToken ct = default)
        {
            var domain = new ExpenseCategory { Name = category };
            await _context.ExpensesCategories.AddAsync(domain, ct);
            await _context.SaveChangesAsync(ct);
            return domain;
        }

        public async Task DeleteExpenseCategoryAsync(int categoryId, CancellationToken ct = default)
        {
            var category = await _context.ExpensesCategories.SingleOrDefaultAsync(x => x.Id == categoryId, ct);
            if (category is null)
            {
                throw new EntityNotFoundException(typeof(ExpenseCategory), categoryId);
            }

            _context.ExpensesCategories.Remove(category);
            await _context.SaveChangesAsync(ct);
        }
    }
}
