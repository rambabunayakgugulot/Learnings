using ExceptionHandlingInBankingAPI.Exceptions; // Imports custom exception types, e.g., InsufficientFundsException, for specialized error handling.

namespace ExceptionHandlingInBankingAPI.Middlewares // Declares the namespace for organizational clarity; groups related middleware classes.
{
    public class ExceptionHandlingMiddleware // Defines a middleware class responsible for catching and handling exceptions globally.
    {
        private readonly RequestDelegate _next; // Stores the next middleware in the pipeline; enables chaining of middleware components.

        public ExceptionHandlingMiddleware(RequestDelegate next) // Constructor receives the next delegate, allowing this middleware to pass control onward.
        {
            _next = next; // Assigns the provided delegate to the private field for later use.
        }

        public async Task Invoke(HttpContext context) // Main method called for each HTTP request; handles the request and manages exceptions.
        {
            try
            {
                await _next(context); // Passes the HTTP context to the next middleware/component. If no exception occurs, the request proceeds normally.
            }
            catch (InsufficientFundsException ex) // Catches a specific custom exception, allowing for tailored error responses.
            {
                context.Response.StatusCode = 400; // Sets HTTP status code to 400 (Bad Request), indicating a client-side error.
                await context.Response.WriteAsync("Custom error: " + ex.Message); // Returns a custom error message to the client, improving clarity and user experience.
            }
            catch (Exception ex) // Catches any other unhandled exceptions, providing a global safety net.
            {
                context.Response.StatusCode = 500; // Sets HTTP status code to 500 (Internal Server Error), signaling a server-side problem.
                await context.Response.WriteAsync("Global error: " + ex.Message); // Returns a generic error message, preventing sensitive details from leaking.
            }
        }
    }
}
