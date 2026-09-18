using Xunit;
using Xunit.Runner.Common;
using FinalApi.Test.XUnit;

[assembly: TestMethodOrderer(typeof(SequentialTestMethodOrderer))]
[assembly: RegisterRunnerReporter(typeof(CustomReporter))]
