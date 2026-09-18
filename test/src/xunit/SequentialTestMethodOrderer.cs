namespace FinalApi.Test.XUnit
{
    using System.Collections.Generic;
    using Xunit.Sdk;
    using Xunit.v3;

    /*
     * Override defaults to run tests in the order in which I declare them
     */
    public class SequentialTestMethodOrderer : ITestMethodOrderer
    {
        public IReadOnlyCollection<TTestMethod?> OrderTestMethods<TTestMethod>(IReadOnlyCollection<TTestMethod?> testMethods)
            where TTestMethod : notnull, ITestMethod
        {
            return testMethods;
        }
    }
}
