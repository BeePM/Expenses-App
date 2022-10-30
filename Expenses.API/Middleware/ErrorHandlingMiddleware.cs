using Expenses.API.Exceptions;

namespace Expenses.API.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (AuthorizationException aex)
            {
                _logger.LogError(aex, "{Message}", aex.Message);
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
            }
            catch (EntityNotFoundException nfex)
            {
                _logger.LogError(nfex, "{Message}", nfex.Message);
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync(nfex.Message);
            }
            catch (InputValidationException ivex)
            {
                _logger.LogError(ivex, "{Message}", ivex.Message);
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync(ivex.Message);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Unexpected error occurred during request execution: {Error}", ex.Message);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
        }
    }
}
