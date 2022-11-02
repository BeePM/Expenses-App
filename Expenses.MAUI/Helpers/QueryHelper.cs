using System.Text;

namespace Expenses.MAUI.Helpers
{
    internal class QueryHelper
    {
        private readonly string _api;
        private readonly Dictionary<string, string> _queryParameters = new();

        private QueryHelper(string api)
        {
            _api = api;
        }

        internal static QueryHelper Create(string api) => new QueryHelper(api);
        internal QueryHelper AddQueryParameter(string name, string? value) => AddQueryParameterInternal(name, value);
        internal QueryHelper AddQueryParameter(string name, object? value) => AddQueryParameterInternal(name, value?.ToString());
        internal string Build() => BuildInternal();

        private QueryHelper AddQueryParameterInternal(string name, string? value)
        {
            if (value is null)
            {
                return this;
            }

            _ = _queryParameters.TryAdd(name, value);
            return this;
        }

        private string BuildInternal()
        {
            var builder = new StringBuilder(_api);
            if (_queryParameters.Count > 0)
            {
                builder.Append('?');
                var listOfQueryParameters = _queryParameters.ToList();
                for (int i = 0; i < listOfQueryParameters.Count; i++)
                {
                    var item = listOfQueryParameters[i];
                    builder.Append(item.Key).Append('=').Append(item.Value);
                    if (i < listOfQueryParameters.Count - 1)
                    {
                        builder.Append('&');
                    }
                }
            }

            return builder.ToString();
        }
    }
}
