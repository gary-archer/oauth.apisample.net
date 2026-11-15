namespace FinalApi.Middleware.Claims
{
    using System.Security.Claims;

    /*
     * Contains JWT claims from the access token and extra values from a secondary user record
     * Provides a convenient object to inject into business logic, to make all user attributes available
     */
    public class CustomClaimsPrincipal : ClaimsPrincipal
    {
        public CustomClaimsPrincipal(JwtClaims jwtClaims, ExtraClaims extraClaims, ClaimsIdentity identity)
            : base(identity)
        {
            this.Jwt = jwtClaims;
            this.Extra = extraClaims;
        }

        public JwtClaims Jwt { get; private set; }

        public ExtraClaims Extra { get; private set; }
    }
}
