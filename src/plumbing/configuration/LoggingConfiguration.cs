namespace FinalApi.Plumbing.Configuration
{
    using System.Text.Json.Nodes;

    /*
     * Logging configuration settings
     */
    public sealed class LoggingConfiguration
    {
        // The name of the API
        public required string ApiName { get; set; }

        // Configured loggers
        public required JsonArray Loggers { get; set; }
    }
}
