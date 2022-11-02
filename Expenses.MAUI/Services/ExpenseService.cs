using System.Net.Http.Headers;
using System.Net.Http.Json;
using Expenses.Common;
using Expenses.Common.DTO;
using Expenses.MAUI.Extensions;
using Expenses.MAUI.Helpers;
using Expenses.MAUI.Interfaces;

namespace Expenses.MAUI.Services;

public class ExpenseService : IExpenseService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ExpenseService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<ExpenseDTO>?> GetExpensesAsync(DateOnly? startDate = null,
                                                                DateOnly? endDate = null,
                                                                string? category = null,
                                                                int? offset = 0,
                                                                int? limit = Constants.DefaultPageSize)
    {
        using var client = _httpClientFactory.CreateClient("Api");
        client.BaseAddress = new Uri(Constants.ApiUrl);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var uri = QueryHelper.Create("api/expenses")
            .AddQueryParameter(nameof(startDate), startDate.ToDateFormat())
            .AddQueryParameter(nameof(endDate), endDate.ToDateFormat())
            .AddQueryParameter(nameof(category), category)
            .AddQueryParameter(nameof(offset), offset)
            .AddQueryParameter(nameof(limit), limit)
            .Build();

        return await client.GetFromJsonAsync<IEnumerable<ExpenseDTO>>(uri);
    }
}