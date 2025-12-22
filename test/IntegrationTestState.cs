namespace FinalApi.IntegrationTests
{
    using System;
    using FinalApi.Test.Utils;

    /*
     * Contains state that is created once for the whole suite of integration tests
     */
    public class IntegrationTestState : IDisposable
    {
        /*
         * Create infrastructure resources once, when the test run begins
         */
        public IntegrationTestState()
        {
            // Create the mock authorization server, which enables productive API tests
            var useProxy = false;
            this.MockAuthorizationServer = new MockAuthorizationServer(useProxy);
            this.MockAuthorizationServer.Start();

            // Create the API client
            var apiBaseUrl = "https://api.authsamples-dev.com:446";
            this.ApiClient = new ApiClient(apiBaseUrl, useProxy);

            // Create a lock delegation ID for testing
            this.DelegationId = Guid.NewGuid().ToString();
        }

        // Wiremock and a JOSE library act as the mock authorization server
        public MockAuthorizationServer MockAuthorizationServer { get; private set; }

        // The API client
        public ApiClient ApiClient { get; private set; }

        // A mock delegation ID
        public string DelegationId { get; private set; }

        /*
         * Destroy infrastructure resources when the test run ends
         */
        public void Dispose()
        {
            this.MockAuthorizationServer.Stop();
            this.MockAuthorizationServer.Dispose();
        }
    }
}
