namespace Expenses.Common.DTO
{
    public class ExpenseCategoryDTO : IDTO
    {
        public ExpenseCategoryDTO(string category)
        {
            Category = category;
        }

        public string Category { get; }
    }
}
