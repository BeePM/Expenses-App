using System.ComponentModel.DataAnnotations;
using Expenses.Common.DTO;

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
