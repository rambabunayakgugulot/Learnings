using ExceptionHandlingInBankingAPI.Exceptions;

namespace ExceptionHandlingInBankingAPI.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (InsufficientFundsException ex)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Custom error: " + ex.Message);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Global error: " + ex.Message);
            }
        }
    }
}
