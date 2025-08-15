namespace ExceptionHandlingInBankingAPI
{
    // The main class for your application
    public class Program
    {
        // The entry point of your application (like "main" in other languages)
        public static void Main(string[] args)
        {
            // Creates a builder object to help set up your web application
            var builder = WebApplication.CreateBuilder(args);

            // ---------------------------
            // Service Registration Section
            // ---------------------------

            // Adds support for controllers (the classes that handle HTTP requests)
            builder.Services.AddControllers();

            // Adds support for minimal APIs and endpoint discovery
            builder.Services.AddEndpointsApiExplorer();

            // Adds and configures Swagger, a tool for API documentation and testing
            builder.Services.AddSwaggerGen();

            // ---------------------------
            // Build the Application
            // ---------------------------

            // Builds the web application using the settings and services above
            var app = builder.Build();

            // ---------------------------
            // Middleware Configuration Section
            // ---------------------------

            // Checks if the app is running in the "Development" environment
            if (app.Environment.IsDevelopment())
            {
                // If in development, enable Swagger UI (interactive API docs)
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Redirects HTTP requests to HTTPS for security
            app.UseHttpsRedirection();

            // (Optional) Place for custom middleware, e.g., exception handling
            // app.UseMiddleware<ExceptionHandlingMiddleware>();

            // Adds authorization middleware (checks if users are allowed to access endpoints)
            app.UseAuthorization();

            // Maps controller endpoints so they can handle incoming HTTP requests
            app.MapControllers();

            // Starts the web application and begins listening for HTTP requests
            app.Run();
        }
    }
}
