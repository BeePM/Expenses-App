using Expenses.API.Database;
using Expenses.API.Exceptions;
using Expenses.API.Interfaces;
using Expenses.API.Models;
using Expenses.API.Queries;
using Expenses.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace Expenses.API.Services
{
    public partial class ExpenseService : IExpenseService
    {
        private readonly ExpensesContext _context;
        private readonly IUserService _userService;

        public ExpenseService(ExpensesContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public string? CurrentUserId => _userService.GetUserId();

        public async Task<Expense> CreateExpenseAsync(CreateExpenseModel model, int categoryId, CancellationToken ct = default)
        {
            var domain = model.ToDomain();
            domain.CategoryId = categoryId;
            domain.UserId = _userService.GetUserId();

            await _context.Expenses.AddAsync(domain, ct);
            await _context.SaveChangesAsync(ct);
            return domain;
        }

        public async Task DeleteExpenseAsync(int expenseId, CancellationToken ct = default)
        {
            var expense = await _context.Expenses.SingleOrDefaultAsync(x => x.Id == expenseId, ct);
            if (expense is null)
            {
                throw new EntityNotFoundException(typeof(Expense), expenseId);
            }

            AuthorizeExpenseOperation(expense);

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<List<Expense>> GetAllExpensesAsync(GetAllExpensesQuery query, CancellationToken ct = default)
        {
            if (!query.IsValid)
            {
                throw new InputValidationException("EndDate must be greater or at least equals to StartDate");
            }

            var dbQuery = _context.Expenses.AsQueryable()
                .AsNoTracking()
                .Include(x => x.Category)
                .Where(x => x.UserId == CurrentUserId);

            if (query.StartDate.HasValue)
            {
                dbQuery = dbQuery.Where(x => x.ExpenseDate >= query.StartDateTime);
            }

            if (query.EndDate.HasValue)
            {
                dbQuery = dbQuery.Where(x => x.ExpenseDate <= query.EndDateTime);
            }

            if (query.Category is not null)
            {
                dbQuery = dbQuery.Where(x => EF.Functions.Like(x.Category!.Name!, query.Category));
            }

            return await dbQuery
                .Skip(query.Offset)
                .Take(query.Limit)
                .ToListAsync(ct);
        }

        public async Task<Expense> GetExpenseAsync(int expenseId, CancellationToken ct = default)
        {
            var expense = await _context.Expenses
                .AsNoTracking()
                .Include(x => x.Category)
                .SingleOrDefaultAsync(x => x.Id == expenseId && x.UserId == CurrentUserId, ct);

            if (expense is null)
            {
                throw new EntityNotFoundException(typeof(Expense), expenseId);
            }

            return expense;
        }

        public async Task<Expense> UpdateExpenseAsync(UpdateExpenseModel model, int categoryId, CancellationToken ct = default)
        {
            var expense = await _context.Expenses.SingleOrDefaultAsync(x => x.Id == model.ExpenseId, ct);
            if (expense is null)
            {
                throw new EntityNotFoundException(typeof(Expense), model.ExpenseId);
            }

            AuthorizeExpenseOperation(expense);

            model.UpdateDomain(expense);
            _context.Expenses.Update(expense);
            await _context.SaveChangesAsync(ct);
            return expense;
        }
    }
}
