namespace FinalApi.Test.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Security.Cryptography;
    using System.Threading.Tasks;
    using Jose;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    /*
     * A mock authorization server implemented with an HTTPS server and a JOSE library
     */
    public class MockAuthorizationServer
    {
        private readonly WebApplication httpServer;
        private readonly ECDsa keypair;
        private readonly Jwk tokenSigningPrivateKey;
        private readonly Jwk tokenSigningPublicKey;
        private readonly string keyId;

        public MockAuthorizationServer()
        {
            var algorithm = "ES256";
            this.keypair = ECDsa.Create(ECCurve.NamedCurves.nistP256);
            this.keyId = Guid.NewGuid().ToString();

            this.tokenSigningPrivateKey = new Jwk(this.keypair, true);
            this.tokenSigningPublicKey = new Jwk(this.keypair, false)
            {
                Alg = algorithm,
                KeyId = this.keyId,
            };

            var keyset = new JwkSet(this.tokenSigningPublicKey);
            var keysJson = keyset.ToJson(JWT.DefaultSettings.JsonMapper);

            var builder = WebApplication.CreateBuilder();
            builder.Logging.ClearProviders();
            builder.WebHost
                .UseKestrel(options =>
                {
                    options.Listen(IPAddress.Loopback, 447, listenOptions =>
                    {
                        listenOptions.UseHttps("../../../../certs/authsamples-dev.ssl.p12", "Password1");
                    });
                });

            this.httpServer = builder.Build();
            this.httpServer.MapGet("/.well-known/jwks.json", () =>
                Results.Content(keysJson, contentType: "application/json"));
        }

        /*
         * Start the HTTP server
         */
        public async ValueTask StartAsync()
        {
            await this.httpServer.StartAsync();
        }

        /*
         * Stop the HTTP server and free resources
         */
        public async ValueTask DisposeAsync()
        {
            await this.httpServer.StopAsync();
            await this.httpServer.DisposeAsync();
            this.keypair.Dispose();
        }

        /*
         * Issue an access token with the supplied subject claim
         */
        public string IssueAccessToken(MockTokenOptions options, Jwk? jwk = null)
        {
            var now = DateTimeOffset.Now;
            var exp = now.AddMinutes(options.ExpiryMinutes);

            var headers = new Dictionary<string, object>()
            {
                { "kid", this.keyId },
            };

            var payload = new Dictionary<string, object>()
            {
                { "iss", options.Issuer },
                { "aud", options.Audience },
                { "scope", options.Scope },
                { "delegation_id", options.DelegationId },
                { "client_id", "TestClient" },
                { "sub", options.Subject },
                { "manager_id", options.ManagerId },
                { "role", options.Role },
                { "exp", exp.ToUnixTimeSeconds() },
            };

            var jwkToUse = jwk ?? this.tokenSigningPrivateKey;
            return JWT.Encode(payload, jwkToUse, JwsAlgorithm.ES256, headers);
        }
    }
}
