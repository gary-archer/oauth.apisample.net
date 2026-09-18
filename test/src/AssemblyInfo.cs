using FinalApi.Test.XUnit;
using Xunit;
using Xunit.Runner.Common;

[assembly: TestMethodOrderer(typeof(SequentialTestMethodOrderer))]
[assembly: RegisterRunnerReporter(typeof(CustomReporter))]
