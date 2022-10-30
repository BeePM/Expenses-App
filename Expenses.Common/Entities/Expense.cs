using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Expenses.Common.DTO;
using Expenses.Common.Enums;

namespace Expenses.Common.Entities
{
    public class Expense : IToDTO<ExpenseDTO>
    {
        [Key]
        public int Id { get; set; }
        public string? Description { get; set; }
        public ExpenseTypeEnum ExpenseType { get; set; }
        public DateTime ExpenseDate { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Amount { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public virtual ExpenseCategory? Category { get; set; }

        [Required]
        public string? UserId { get; set; }

        public ExpenseDTO ToDTO() => new(Id,
                                         Description,
                                         ExpenseType,
                                         ExpenseDate,
                                         Amount,
                                         Category!.Name!);
    }
}
