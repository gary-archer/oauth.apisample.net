namespace FinalApi.Plumbing.Logging
{
    /*
     * A log entry collects data during an API request and outputs it at the end
     */
    public interface ILogEntry
    {
        // Create a performance breakdown
        IPerformanceBreakdown CreatePerformanceBreakdown(string name);

        string GetSessionId();
    }
}
