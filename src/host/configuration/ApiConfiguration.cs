namespace FinalApi.Host.Configuration
{
    /*
     * Application specific settings
     */
    public class ApiConfiguration
    {
        public required int Port { get; set; }

        public required string SslCertificateFileName { get; set; }

        public required string SslCertificatePassword { get; set; }

        public required bool UseProxy { get; set; }

        public required string ProxyUrl { get; set; }
    }
}
