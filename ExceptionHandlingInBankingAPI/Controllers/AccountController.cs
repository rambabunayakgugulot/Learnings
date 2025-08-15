using ExceptionHandlingInBankingAPI.Exceptions;
using ExceptionHandlingInBankingAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExceptionHandlingInBankingAPI.Controllers
{

    [Route("api/[controller]")]
    public class AccountController : ControllerBase //ControllerBase provides all the action result helpers (Ok(), BadRequest(), etc.).
    {
        [HttpPost("withdraw")]
        public IActionResult Withdraw([FromBody] WithdrawalRequest request)
        {
            decimal balance = 100;
            try
            {
                // 5. Throwing Exception for invalid input
                if (request.Amount <= 0)
                    throw new ArgumentOutOfRangeException(nameof(request.Amount), "Amount must be positive");

                // 6. Custom Exception for domain-specific error
                if (!AccountExists(request.AccountId))
                    throw new InvalidAccountException("Account does not exist.");

                if (request.Amount > balance)
                    throw new InsufficientFundsException("Not enough funds for withdrawal.");

                decimal newBalance = balance - request.Amount;
                return Ok(new { NewBalance = newBalance });
            }
            catch (InvalidAccountException ex)
            {
                // Reviewer: Specific handling for domain error.
                return NotFound(ex.Message);
            }
            catch (InsufficientFundsException ex)
            {
                // Reviewer: Specific handling for domain error.
                return BadRequest(ex.Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                // Reviewer: Specific handling for bad input.
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Reviewer: Log and rethrow for global handling.
                LogError(ex);
                throw; // Best practice: preserves stack trace -- if you comment InvalidAccountException catch block - throw will show you line no. = 23 if invalid account id passed where as throw ex will show line no. =50 as if the catch block caused excpetions
            }
            finally
            {
                // Reviewer: Always log the transaction attempt.
                Console.WriteLine($"Withdrawal attempted at {DateTime.Now}");
            }
        }

        private bool AccountExists(string accountId)
        {
            // Reviewer: Simulate account check (replace with real DB logic)
            return accountId == "12345";
        }

        private void LogError(Exception ex)
        {
            // Reviewer: Centralized error logging.
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

