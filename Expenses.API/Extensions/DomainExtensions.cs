using Expenses.Common;

namespace Expenses.API.Extensions
{
    public static class DomainExtensions
    {
        public static IEnumerable<T> ProjectToDTOCollection<T>(this IEnumerable<IToDTO<T>> response) where T : IDTO
            => response.Select(x => x.ToDTO()).ToList();
    }
}
