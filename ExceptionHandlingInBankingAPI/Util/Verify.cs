namespace ExceptionHandlingInBankingAPI.Util
{
    public static class Verify
    {
        public static void NotNullOrWhiteSpace(string paramName, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"{paramName} cannot be null or whitespace.", paramName);
        }

        public static void GreaterThanZero(string paramName, decimal value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(paramName, $"{paramName} must be greater than zero.");
        }
    }
}
