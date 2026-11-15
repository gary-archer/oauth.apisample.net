namespace FinalApi.Middleware.Logging
{
    using System;
    using FinalApi.Configuration;
    using log4net;
    using Microsoft.Extensions.Logging;

    /*
     * An interface to create and get logger objects
     */
    public interface ILoggerFactory
    {
        // Configure logging at startup
        void Configure(ILoggingBuilder builder, LoggingConfiguration configuration);

        // Log API startup errors
        void LogStartupError(Exception exception);

        // Get the request logger
        ILog? GetRequestLogger();

        // Get the audit logger
        ILog? GetAuditLogger();

        // Get a named debug logger
        ILog? GetDebugLogger(string name);
    }
}
