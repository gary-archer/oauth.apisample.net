namespace FinalApi.Plumbing.Claims
{
    using System.Collections.Generic;
    using System.Text.Json.Nodes;

    /*
     * The claims from the JWT access token
     */
    public class JwtClaims
    {
        /*
         * Wrap the raw payload
         */
        public JwtClaims(string claimsJson)
        {
            this.Payload = JsonNode.Parse(claimsJson);
        }

        /*
         * The payload
         */
        public JsonNode Payload { get; private set; }

        /*
         * Get audiences as an array
         */
        public IEnumerable<string> GetAudiences()
        {
            var results = new List<string>();

            var audienceNode = this.Payload[ClaimNames.Audience];
            if (audienceNode != null)
            {
                var audiences = audienceNode as JsonArray;
                if (audiences != null)
                {
                    foreach (var audience in audiences)
                    {
                        results.Add(audience.GetValue<string>());
                    }
                }
                else
                {
                    results.Add(audienceNode.GetValue<string>());
                }
            }

            return results;
        }
    }
}
