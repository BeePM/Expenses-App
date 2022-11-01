using System.ComponentModel.DataAnnotations;
using Expenses.Common;

namespace Expenses.API.Queries
{
    public class PagingQuery
    {
        private int? _offset;
        private int? _limit;

        public int Offset
        {
            get => _offset == null || _offset < 0 ? 0 : _offset.Value;
            set => _offset = value;
        }

        [Range(1, 1000)]
        public int Limit
        {
            get => _limit ?? Constants.DefaultPageSize;
            set => _limit = value;
        }
    }
}
