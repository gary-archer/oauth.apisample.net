namespace FinalApi.Test.XUnit
{
    using System.Linq;
    using Xunit.Runner.Common;
    using Xunit.Sdk;

    /*
     * Display passed tests in green and failed tests in red
     */
    public class CustomReporterMessageHandler : TestMessageSink, IRunnerReporterMessageHandler
    {
        private readonly IRunnerLogger logger;
        private readonly MessageMetadataCache metadataCache;

        public CustomReporterMessageHandler(IRunnerLogger logger)
        {
            this.logger = logger;
            this.metadataCache = new MessageMetadataCache();
            this.Execution.TestStartingEvent += this.OnTestStarting;
            this.Execution.TestPassedEvent += this.OnTestPassed;
            this.Execution.TestFailedEvent += this.OnTestFailed;
        }

        private void OnTestStarting(MessageHandlerArgs<ITestStarting> args)
        {
            this.metadataCache.Set(args.Message);
        }

        private void OnTestPassed(MessageHandlerArgs<ITestPassed> args)
        {
            var test = args.Message;
            var metadata = this.metadataCache.TryGetTestMetadata(test);
            if (metadata != null && metadata.TestDisplayName != "RunLoadTest")
            {
                var yellow = "\u001b[93m";
                var green = "\u001b[32m";
                if (metadata != null)
                {
                    this.logger.LogMessage($"    {yellow}[test] {green}{metadata.TestDisplayName} PASSED ✓");
                }
            }
        }

        private void OnTestFailed(MessageHandlerArgs<ITestFailed> args)
        {
            var yellow = "\u001b[93m";
            var red = "\u001b[31m";
            var test = args.Message;
            var metadata = this.metadataCache.TryGetTestMetadata(test);
            if (metadata != null)
            {
                this.logger.LogMessage($"    {yellow}[test] {red}{metadata.TestDisplayName} FAILED ✗: {string.Join(',', test.Messages)}");
            }
        }
    }
}
