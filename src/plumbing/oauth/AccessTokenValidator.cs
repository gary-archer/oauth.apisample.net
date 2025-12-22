namespace FinalApi.Plumbing.OAuth
{
    using System;
    using System.ComponentModel;
    using System.Data.Common;
    using System.Diagnostics;
    using System.IO.Compression;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text.Json.Nodes;
    using System.Threading.Tasks;
    using FinalApi.Plumbing.Claims;
    using FinalApi.Plumbing.Configuration;
    using FinalApi.Plumbing.Errors;
    using FinalApi.Plumbing.Logging;
    using Jose;

    /*
     * A class to verify the JWT access token, to authenticate the request
     */
    public sealed class AccessTokenValidator
    {
        private readonly OAuthConfiguration configuration;
        private readonly JsonWebKeyResolver jsonWebKeyResolver;
        private readonly LogEntry logEntry;

        public AccessTokenValidator(
            OAuthConfiguration configuration,
            JsonWebKeyResolver jsonWebKeyResolver,
            ILogEntry logEntry)
        {
            this.configuration = configuration;
            this.jsonWebKeyResolver = jsonWebKeyResolver;
            this.logEntry = (LogEntry)logEntry;
        }

        /*
         * Validate the access token using the jose-jwt library
         */
        public async Task<JwtClaims> ValidateTokenAsync(string accessToken)
        {
            using (this.logEntry.CreatePerformanceBreakdown("tokenValidator"))
            {
                JwtClaims claims = null;
                string claimsJson = string.Empty;
                try
                {
                    // Read the token without validating it, to get its key identifier
                    var kid = this.GetKeyIdentifier(accessToken);
                    if (kid == null)
                    {
                        throw ErrorFactory.CreateClient401Error("Unable to read the kid field from the access token");
                    }

                    // Get the token signing public key as a JSON web key
                    var jwk = await this.jsonWebKeyResolver.GetTokenSigningPublicKey(kid, this.configuration.Algorithm);
                    if (jwk == null)
                    {
                        throw ErrorFactory.CreateClient401Error(
                            $"The token kid {kid} was not found in the JWKS for algorithm {this.configuration.Algorithm}");
                    }

                    // Do the cryptographic validation of the JWT signature using the JWK public key
                    claimsJson = JWT.Decode(accessToken, jwk);
                    claims = new JwtClaims(claimsJson);

                    // Verify the protocol claims according to best practices
                    this.ValidateProtocolClaims(claims);

                    // Add identity data to logs
                    this.logEntry.SetIdentityData(this.GetIdentityData(claims));
                }
                catch (Exception ex)
                {
                    // For expired access tokens, add identity data to logs
                    if (claims != null && this.IsExpired(claims))
                    {
                        this.logEntry.SetIdentityData(this.GetIdentityData(claims));
                    }

                    // Do the same for my expired access token testing, which causes invalid signatures
                    if (ex is IntegrityException)
                    {
                        claimsJson = JWT.Payload(accessToken);
                        claims = new JwtClaims(claimsJson);
                        this.logEntry.SetIdentityData(this.GetIdentityData(claims));
                    }

                    // Otherwise return a 401 error
                    throw ErrorUtils.FromTokenValidationError(ex);
                }

                return claims;
            }
        }

        /*
         * Read the kid field from the JWT header
         */
        private string GetKeyIdentifier(string accessToken)
        {
            var headers = JWT.Headers(accessToken);
            if (headers.ContainsKey("kid"))
            {
                return headers["kid"] as string;
            }

            return null;
        }

        /*
        * Collect identity data to add to logs
        */
        private IdentityLogData GetIdentityData(JwtClaims claims)
        {
            var data = new IdentityLogData()
            {
                UserId = claims.GetStringClaim(ClaimNames.Subject),
                SessionId = claims.GetStringClaim(this.configuration.SessionIDClaimName),
                ClientId = claims.GetStringClaim(ClaimNames.ClientId),
                Scope = claims.GetStringClaim(ClaimNames.Scope),
                Claims = new JsonObject
                {
                    ["managerId"] = claims.GetStringClaim(ClaimNames.ManagerId),
                    ["role"] = claims.GetStringClaim(ClaimNames.Role),
                },
            };

            return data;
        }

        /*
         * jose-jwt does not support checking standard claims for issuer, audience and expiry, so make those checks here instead
         */
        private void ValidateProtocolClaims(JwtClaims claims)
        {
            // Check the expected issuer
            if (claims.Iss != this.configuration.Issuer)
            {
                throw ErrorFactory.CreateClient401Error("The issuer claim had an unexpected value");
            }

            // Check the expected audience, and Cognito does not issue an audience claim to access tokens
            if (!string.IsNullOrWhiteSpace(this.configuration.Audience))
            {
                var audiences = claims.GetAudiences();
                if (!audiences.Contains(this.configuration.Audience))
                {
                    throw ErrorFactory.CreateClient401Error("The audience claim had an unexpected value");
                }
            }

            // Check that the JWT is not expired
            if (this.IsExpired(claims))
            {
                throw ErrorFactory.CreateClient401Error("The access token is expired");
            }
        }

        /*
         * See if the token has already expired
         */
        private bool IsExpired(JwtClaims claims)
        {
            return claims.Exp < DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
    }
}
