namespace FinalApi.Plumbing.Configuration
{
    /*
     * Configuration settings to enable standard security and extensible use of claims
     */
    public sealed class OAuthConfiguration
    {
        // The expected issuer in JWT access tokens received
        public required string Issuer { get; set; }

        // The expected audience in JWT access tokens received
        public required string Audience { get; set; }

        // The expected algorithm in JWT access tokens received
        public required string Algorithm { get; set; }

        // A required scope to call the API
        public required string Scope { get; set; }

        // The endpoint from which to download the token signing public key
        public required string JwksEndpoint { get; set; }

        // The access token claim that the API uses as a session ID
        public required string DelegationIDClaimName { get; set; }

        // Optional claims caching configuration
        public required int ClaimsCacheTimeToLiveMinutes { get; set; }
    }
}
