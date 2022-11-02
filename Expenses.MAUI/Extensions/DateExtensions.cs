namespace Expenses.MAUI.Extensions
{
    public static class DateExtensions
    {
        public static string? ToDateFormat(this DateTime? dateTime) => dateTime?.ToString("yyyy-MM-dd");
        public static string? ToDateFormat(this DateOnly? date) => date?.ToString("yyyy-MM-dd");
    }
}
