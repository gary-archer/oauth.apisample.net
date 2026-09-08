namespace FinalApi.Test.Reporter
{
    using System.Threading.Tasks;
    using Xunit.Runner.Common;
    using Xunit.Sdk;

    /*
     * Control xunit test output
     */
    public class CustomReporter : IRunnerReporter
    {
        public string Description => "Custom test reporter";

        public bool CanBeEnvironmentallyEnabled => false;

        public bool ForceNoLogo => false;

        public bool IsEnvironmentallyEnabled => false;

        public string RunnerSwitch => "custom";

        public ValueTask<IRunnerReporterMessageHandler> CreateMessageHandler(
            IRunnerLogger logger,
            IMessageSink diagnosticMessageSink)
        {
            return new ValueTask<IRunnerReporterMessageHandler>(
                new CustomReporterMessageHandler(logger)
            );
        }
    }
}
