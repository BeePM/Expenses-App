using System.Text.Json.Serialization;

namespace Expenses.API.Queries
{
    public class GetAllExpensesQuery : PagingQuery
    {
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string? Category { get; set; }

        [JsonIgnore]
        internal bool IsValid => EndDate == null && StartDate == null || EndDate >= StartDate;

        [JsonIgnore]
        internal DateTime? StartDateTime => StartDate?.ToDateTime(TimeOnly.Parse("00:00:00"));

        [JsonIgnore]
        internal DateTime? EndDateTime => EndDate?.ToDateTime(TimeOnly.Parse("00:00:00"));
    }
}
