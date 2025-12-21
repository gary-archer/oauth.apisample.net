namespace FinalApi.Plumbing.Logging
{
    using System.Text.Json.Nodes;

    /*
     * Represents extra authorization values not received in access tokens
     */
    public class IdentityLogData
    {
        public string UserId { get; set; }

        public string SessionId { get; set; }

        public string ClientId { get; set; }

        public string Scope { get; set; }

        public JsonNode Claims { get; set; }
    }
}
