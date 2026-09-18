namespace FinalApi.Plumbing.Logging
{
    using System.Text.Json.Nodes;

    /*
     * Represents extra authorization values not received in access tokens
     */
    public class IdentityLogData
    {
        public required string UserId { get; set; }

        public required string DelegationId { get; set; }

        public required string ClientId { get; set; }

        public required string Scope { get; set; }

        public required JsonNode Claims { get; set; }
    }
}
