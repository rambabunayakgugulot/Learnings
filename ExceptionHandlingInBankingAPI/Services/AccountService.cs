using ExceptionHandlingInBankingAPI.Util;

namespace ExceptionHandlingInBankingAPI.Services
{
    public class AccountService
    {
        private readonly ILogger<AccountService> _logger;

        public AccountService(ILogger<AccountService> logger)
        {
            _logger = logger;
        }

        public void Withdraw(string accountId, decimal amount)
        {
            // Input validation using central utility
            Verify.NotNullOrWhiteSpace(nameof(accountId), accountId);
            Verify.GreaterThanZero(nameof(amount), amount);

            try
            {
                // Business logic (e.g., check balance, update account)
                // For demonstration, let's simulate a possible business error:
                if (amount > 1000) // Simulate insufficient funds
                    throw new InvalidOperationException("Insufficient funds.");

                // ...withdraw logic...
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Withdraw for AccountId: {AccountId}", accountId);
                throw; // Rethrow to let controller or middleware handle
            }
        }
    }
}
