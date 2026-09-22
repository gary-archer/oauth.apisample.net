namespace FinalApi.Test
{
    using System;
    using System.Threading.Tasks;
    using FinalApi.Test.Utils;
    using Xunit;

    /*
     * The fixture contains state during a test run
     */
    public class OAuthTestFixture : IAsyncLifetime
    {
        /*
         * Create infrastructure resources once, when the test run begins
         */
        public OAuthTestFixture()
        {
            // Create the mock authorization server
            this.MockAuthorizationServer = new MockAuthorizationServer();

            // Create the API client
            var useProxy = false;
            var apiBaseUrl = "https://api.authsamples-dev.com:446";
            this.ApiClient = new ApiClient(apiBaseUrl, useProxy);

            // Create a lock delegation ID for testing
            this.DelegationId = Guid.NewGuid().ToString();
        }

        // An HTTPS server and a JOSE library act as the mock authorization server
        public MockAuthorizationServer MockAuthorizationServer { get; private set; }

        // The API client
        public ApiClient ApiClient { get; private set; }

        // A mock delegation ID
        public string DelegationId { get; private set; }

        /*
         * Start the HTTPS server at the beginning of a test run
         */
        public async ValueTask InitializeAsync()
        {
            await this.MockAuthorizationServer.StartAsync();
        }

        /*
         * Dipose the HTTPS server at the end of a test run
         */
        public async ValueTask DisposeAsync()
        {
            await this.MockAuthorizationServer.DisposeAsync();
        }
    }
}
