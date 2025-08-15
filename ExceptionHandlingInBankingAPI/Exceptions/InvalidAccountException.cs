namespace ExceptionHandlingInBankingAPI.Exceptions
{
    public class InvalidAccountException : Exception
    {
        public InvalidAccountException(string message) : base(message)
        {
            
        }
    }
}
