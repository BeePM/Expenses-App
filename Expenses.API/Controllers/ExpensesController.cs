using System.ComponentModel.DataAnnotations;
using Expenses.API.Extensions;
using Expenses.API.Interfaces;
using Expenses.API.Models;
using Expenses.API.Queries;
using Expenses.Common.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Expenses.API.Controllers;

/// <summary>
/// Management of expenses
/// </summary>
[ApiController]
[Route("api/[controller]")]
//[Authorize]
[Produces("application/json")]
public class ExpensesController : ControllerBase
{
    private readonly IExpenseService _expenseService;
    private const string GetExpenseRoute = "GetExpense";

    /// <summary>
    /// Creates new ExpensesController
    /// </summary>
    /// <param name="expenseService">service</param>
    public ExpensesController(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    /// <summary>
    /// Query expenses based on input criteria containing StartDate and optional EndDate
    /// </summary>
    /// <param name="query">query specification</param>
    /// <param name="ct">cancellation</param>
    /// <returns>Collection of queried expenses for given input criteria</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ExpenseDTO>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IEnumerable<ExpenseDTO>> GetExpensesAsync([FromQuery] GetAllExpensesQuery query,
                                                                CancellationToken ct)
    {
        var response = await _expenseService.GetAllExpensesAsync(query, ct);
        return response.ProjectToDTOCollection();
    }

    /// <summary>
    /// Select specific expense based on its identity
    /// </summary>
    /// <param name="expenseId">expense identity</param>
    /// <param name="ct">cancellation</param>
    /// <returns>Expense</returns>
    [HttpGet("{expenseId:int}", Name = GetExpenseRoute)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ExpenseDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<ExpenseDTO> GetExpenseAsync([FromRoute, Required] int expenseId,
                                                  CancellationToken ct)
    {
        var response = await _expenseService.GetExpenseAsync(expenseId, ct);
        return response.ToDTO();
    }

    /// <summary>
    /// Creates new expense and also new category if it doesn't exists
    /// </summary>
    /// <param name="model">model</param>
    /// <param name="ct">cancellation</param>
    /// <returns>Created expense</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ExpenseDTO))]
    public async Task<ActionResult<ExpenseDTO>> CreateExpenseAsync([FromBody, Bind] CreateExpenseModel model,
                                                                   CancellationToken ct)
    {
        var category = await _expenseService.FindExpenseCategoryAsync(model.Category!, ct);
        category ??= await _expenseService.CreateExpenseCategoryAsync(model.Category!, ct);

        var response = await _expenseService.CreateExpenseAsync(model, category.Id, ct);
        return CreatedAtAction(GetExpenseRoute, new { expenseId = response.Id }, response.ToDTO());
    }

    /// <summary>
    /// Updates existing expense with new metadata and also new category will be created if it doesn't exists
    /// </summary>
    /// <param name="model">model</param>
    /// <param name="ct">cancellation</param>
    /// <returns>Modified expense</returns>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ExpenseDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ExpenseDTO> UpdateExpenseAsync([FromBody, Bind] UpdateExpenseModel model,
                                                     CancellationToken ct)
    {
        var category = await _expenseService.FindExpenseCategoryAsync(model.Category!, ct);
        category ??= await _expenseService.CreateExpenseCategoryAsync(model.Category!, ct);

        var response = await _expenseService.UpdateExpenseAsync(model, category.Id, ct);
        return response.ToDTO();
    }

    /// <summary>
    /// Deletes expense
    /// </summary>
    /// <param name="expenseId">expense identity</param>
    /// <param name="ct">cancellation</param>
    /// <returns></returns>
    [HttpDelete("{expenseId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> DeleteExpenseAsync([FromRoute, Required] int expenseId,
                                                 CancellationToken ct)
    {
        await _expenseService.DeleteExpenseAsync(expenseId, ct);
        return NoContent();
    }
}
