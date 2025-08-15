using ExceptionHandlingInBankingAPI.Models;
using ExceptionHandlingInBankingAPI.Services;
using ExceptionHandlingInBankingAPI.Util;
using Microsoft.AspNetCore.Mvc;

namespace ExceptionHandlingInBankingAPI.Controllers
{
    public class AccountRefactoredController :ControllerBase
    {
        private readonly AccountService _accountService;
        private readonly ILogger<AccountController> _logger;

        public AccountRefactoredController(AccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [HttpPost("withdraw")]
        public IActionResult Withdraw([FromBody] WithdrawalRequest request)
        {
            // Input validation at boundary (fail fast)
            if (request == null)
                return BadRequest("Request body is missing.");

            try
            {
                Verify.NotNullOrWhiteSpace(nameof(request.AccountId), request.AccountId);
                Verify.GreaterThanZero(nameof(request.Amount), request.Amount);

                _accountService.Withdraw(request.AccountId, request.Amount);
                return Ok("Withdrawal successful.");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }            
            catch (InvalidOperationException ex)
            {
                // Business rule violation (e.g., insufficient funds)
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception in Withdraw.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
