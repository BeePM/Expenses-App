using Expenses.API.Validation;
using Expenses.Common.Entities;

namespace Expenses.API.Models
{
    public class UpdateExpenseModel : CreateExpenseModel
    {
        [Id]
        public int ExpenseId { get; set; }

        public void UpdateDomain(Expense domain)
        {
            domain.Description = Description;
            domain.ExpenseDate = ExpenseDate;
            domain.ExpenseType = ExpenseType;
            domain.Amount = Amount;
        }
    }
}
