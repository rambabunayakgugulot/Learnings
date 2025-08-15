namespace ExceptionHandlingInBankingAPI.Exceptions
{
    public class InsufficientFundsException : Exception
    {
        public InsufficientFundsException(string message) : base(message)
        {
            
        }
    }
}
