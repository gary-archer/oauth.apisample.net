namespace FinalApi.Plumbing.Middleware
{
    using System.Threading.Tasks;
    using FinalApi.Plumbing.Configuration;
    using FinalApi.Plumbing.Errors;
    using FinalApi.Plumbing.Logging;
    using FinalApi.Plumbing.Utilities;
    using Microsoft.AspNetCore.Http;

    /*
     * A class to handle custom header logic
     */
    public sealed class CustomHeaderMiddleware
    {
        private readonly RequestDelegate next;

        /*
         * Store a reference to the next middleware
         */
        public CustomHeaderMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        /*
         * Handle any special custom headers
         */
        public async Task Invoke(HttpContext context, LoggingConfiguration configuration)
        {
            // Cause a 500 error if a special header is received
            var apiToBreak = context.Request.GetHeader("api-exception-simulation");
            if (!string.IsNullOrWhiteSpace(apiToBreak))
            {
                if (apiToBreak.ToLowerInvariant() == configuration.ApiName.ToLowerInvariant())
                {
                    throw ErrorFactory.CreateServerError(BaseErrorCodes.ExceptionSimulation, "An exception was simulated in the API");
                }
            }

            // Run subsequent handlers including the controller operation
            await this.next(context);

            // Add the session ID to response headers so that the frontend can read and display it
            var logEntry = (LogEntry)context.RequestServices.GetService(typeof(ILogEntry));
            var sessionId = logEntry.GetSessionId();
            if (!string.IsNullOrWhiteSpace(sessionId))
            {
                // context.Response.Headers["session-id"] = sessionId;
            }
        }
    }
}
