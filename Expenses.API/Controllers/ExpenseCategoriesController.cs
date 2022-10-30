using Expenses.API.Extensions;
using Expenses.API.Interfaces;
using Expenses.Common.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Expenses.API.Controllers;

/// <summary>
/// Management of expense categories
/// </summary>
[ApiController]
[Route("api/categories")]
[Authorize]
[Produces("application/json")]
public class ExpenseCategoriesController : ControllerBase
{
    private readonly IExpenseService _expenseService;

    /// <summary>
    /// Creates new ExpenseCategoriesController
    /// </summary>
    /// <param name="expenseService">service</param>
    public ExpenseCategoriesController(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    /// <summary>
    /// Query expense categories
    /// </summary>
    /// <param name="ct">cancellation</param>
    /// <returns>Collection of queried expense categories</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ExpenseCategoryDTO>))]
    public async Task<IEnumerable<ExpenseCategoryDTO>> GetExpenseCategoriesAsync(CancellationToken ct)
    {
        var response = await _expenseService.GetExpenseCategoriesAsync(ct);
        return response.ProjectToDTOCollection();
    }
}

