namespace FinalApi.Middleware.Claims
{
    using System;
    using System.Threading.Tasks;
    using FinalApi.Logic.Repositories;
    using FinalApi.Middleware.Claims;

    /*
     * Add extra authorization values that you cannot, or do not want to, manage in the authorization server
     */
    public class ExtraClaimsProvider
    {
        private readonly UserRepository userRepository;

        public ExtraClaimsProvider(UserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        /*
         * Get extra values from the API's own data
         */
        #pragma warning disable 1998
        public async Task<ExtraClaims> LookupExtraClaimsAsync(JwtClaims jwtClaims, IServiceProvider serviceProvider)
        {
            var managerId = ClaimsReader.GetStringClaim(jwtClaims, ClaimNames.ManagerId);
            return this.userRepository.GetUserInfoForManagerId(managerId);
        }
        #pragma warning restore 1998
    }
}
