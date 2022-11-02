using Expenses.Common.Enums;

namespace Expenses.Common.DTO
{
    public class ExpenseDTO : IDTO
    {
        public ExpenseDTO(int expenseId,
                          string? description,
                          ExpenseTypeEnum expenseType,
                          DateTime expenseDate,
                          decimal amount,
                          string category)
        {
            ExpenseId = expenseId;
            Description = description;
            ExpenseType = expenseType;
            ExpenseDate = expenseDate;
            Amount = amount;
            Category = category;
        }

        public int ExpenseId { get; set; }
        public string? Description { get; }
        public ExpenseTypeEnum ExpenseType { get; }
        public DateTime ExpenseDate { get; }
        public decimal Amount { get; }
        public string Category { get; }
    }
}
