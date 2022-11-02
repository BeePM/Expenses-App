using System.ComponentModel.DataAnnotations;
using Expenses.Common.Entities;
using Expenses.Common.Enums;

namespace Expenses.API.Models
{
    public class CreateExpenseModel
    {
        public string? Description { get; set; }

        [EnumDataType(typeof(ExpenseTypeEnum))]
        public ExpenseTypeEnum ExpenseType { get; set; }

        [Required]
        public DateTime ExpenseDate { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        public string? Category { get; set; }

        public Expense ToDomain()
        {
            return new Expense
            {
                Amount = Amount,
                Description = Description,
                ExpenseDate = ExpenseDate,
                ExpenseType = ExpenseType
            };
        }
    }
}
