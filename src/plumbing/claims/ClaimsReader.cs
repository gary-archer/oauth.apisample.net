namespace FinalApi.Plumbing.Claims
{
    using FinalApi.Plumbing.Errors;

    /*
     * A simple utility class to read claim values safely
     */
    public static class ClaimsReader
    {
        /*
         * Return a mandatory string claim
         */
        public static string GetStringClaim(JwtClaims claims, string name, bool required = true)
        {
            var claim = claims.Payload[name]?.GetValue<string>();
            if (claim == null)
            {
                if (required)
                {
                    throw ErrorUtils.FromMissingClaim(name);
                }
                else
                {
                    return string.Empty;
                }
            }

            return claim;
        }

        /*
         * Return a mandatory integer claim
         */
        public static int GetIntegerClaim(JwtClaims claims, string name, bool required = true)
        {
            var claim = claims.Payload[name]?.GetValue<int>();
            if (claim == null)
            {
                if (required)
                {
                    throw ErrorUtils.FromMissingClaim(name);
                }
                else
                {
                    return 0;
                }
            }

            return claim.Value;
        }
    }
}
