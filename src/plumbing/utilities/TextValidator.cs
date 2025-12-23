namespace FinalApi.Plumbing.Utilities
{
    using System.Text.RegularExpressions;

    /*
     * Text validation utilities
     */
    public static class TextValidator
    {
        /*
         * Sanitize input text such as correlation IDs and reject suspicious input
         */
        public static string Sanitize(string input)
        {
            if (new Regex(@"^[a-zA-Z0-9-]+$").IsMatch(input) && input.Length <= 64)
            {
                return input;
            }

            return string.Empty;
        }
    }
}
