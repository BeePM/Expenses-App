using Expenses.Common.DTO;
using System.ComponentModel.DataAnnotations;

namespace Expenses.Common.Entities
{
    public class ExpenseCategory : IToDTO<ExpenseCategoryDTO>
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public ExpenseCategoryDTO ToDTO() => new(Name!);
    }
}
