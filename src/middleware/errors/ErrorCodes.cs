namespace FinalApi.Middleware.Errors
{
    /*
     * Error codes to represent error causes
     */
    public static class ErrorCodes
    {
        public static readonly string ServerError = "server_error";

        public static readonly string InvalidToken = "invalid_token";

        public static readonly string TokenSigningKeysDownloadError = "jwks_download_failure";

        public static readonly string InsufficientScope = "insufficient_scope";

        public static readonly string InsufficientData = "insufficient_data";

        public static readonly string ExceptionSimulation = "exception_simulation";

        public static readonly string InvalidCompanyId = "invalid_company_id";

        public static readonly string CompanyNotFound = "company_not_found";

        public static readonly string FileReadError = "file_read_error";
    }
}
