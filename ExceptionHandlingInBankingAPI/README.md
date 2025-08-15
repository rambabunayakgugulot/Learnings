BankingApi – Exception Handling in C# with Real-Time Use Case 🚀
This project demonstrates exception handling in C# using a banking withdrawal API.
Each concept is introduced step by step, with code samples, inline comments, and reviewer notes explaining why, what, how, and what happens if omitted.

📚 **Table of Contents**

Use Case Overview
What Are Exceptions?
Basic Exception Handling: try-catch
Finally Block
Throwing Exceptions
Custom Exceptions & Domain-Specific Errors
Throw vs Rethrow
Real-Time System Design: With vs. Without Exception Handling
Best Practices for Experienced Developers
Running the Demo
Code Review Highlights


**1. Use Case Overview**
A customer wants to withdraw money from their bank account.
We need to handle:

Invalid withdrawal amounts
Insufficient funds
Invalid accounts (nonexistent, locked, etc.)
Unexpected errors (e.g., database issues)


**2. What Are Exceptions?**
Exceptions are errors that disrupt normal program flow.
Example: Trying to withdraw more money than available.

**3. Basic Exception Handling: try-catch**
Before (no handling):

public IActionResult Withdraw(decimal amount)
{
    decimal balance = 100;
    decimal newBalance = balance - amount; // May go negative!
    return Ok(newBalance);
}

**Reviewer Note:**
No error handling—negative balances possible, bad UX!
After (with try-catch):

public IActionResult Withdraw(decimal amount)
{
    try
    {
        decimal balance = 100;
        if (amount > balance)
            throw new Exception("Insufficient funds");
        decimal newBalance = balance - amount;
        return Ok(newBalance);
    }
    catch (Exception ex)
    {
        // Reviewer: Now the user gets a friendly error, not a crash!
        return BadRequest(ex.Message);
    }
}

Why?
Without try-catch, any error would crash the API and confuse users.

**4. Finally Block**
Use Case: Always log the transaction attempt, even if it fails.

public IActionResult Withdraw(decimal amount)
{
    string log = "";
    try
    {
        // ...withdraw logic
    }
    catch (Exception ex)
    {
        log = "Withdrawal failed: " + ex.Message;
        return BadRequest(ex.Message);
    }
    finally
    {
        // Reviewer: Ensures logging happens regardless of success/failure.
        Console.WriteLine("Transaction attempted at " + DateTime.Now);
    }
}

Why?
Without finally, you risk missing logs for failed transactions.

**5. Throwing Exceptions**
**Use Case: Invalid withdrawal amount.**

public IActionResult Withdraw(decimal amount)
{
    if (amount <= 0)
        // Reviewer: Throwing built-in exception for invalid input.
        throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive");
    // ...rest of logic
}

Why?
Throwing exceptions for invalid input prevents bad data from corrupting your system.

**6. Custom Exceptions & Domain-Specific Errors**
**Why Custom Exceptions?**
Custom exceptions represent errors unique to your business logic (“domain”).
They make your code more readable, maintainable, and allow targeted error handling.
Domain-specific errors are problems that only make sense in your application’s context (not generic programming errors).
Examples:

InsufficientFundsException: Only makes sense in banking.
InvalidAccountException: Only makes sense in systems with accounts.

**Code Example:**

public class InsufficientFundsException : Exception
{
    public InsufficientFundsException(string message) : base(message) { }
}

public class InvalidAccountException : Exception
{
    public InvalidAccountException(string message) : base(message) { }
}

Usage in Controller:

if (!AccountExists(request.AccountId))
    throw new InvalidAccountException("Account does not exist.");

if (request.Amount > balance)
    throw new InsufficientFundsException("Not enough funds for withdrawal.");

**Reviewer Note:**

Using custom exceptions makes it clear what went wrong and allows the middleware to return precise error codes/messages.
If you use only generic exceptions, you lose clarity and can’t easily distinguish between business errors and system errors.

What if not used?

All errors look the same (generic), making debugging and user experience worse.
You might accidentally handle system errors (like NullReferenceException) as business errors.


**7. Throw vs Rethrow
What’s the Difference?**


throw ex;
Throws a new exception, resets the stack trace (bad for debugging).


throw;
Rethrows the current exception, preserves the original stack trace (best practice).


**Example:**

try
{
    // ...withdraw logic
}
catch (Exception ex)
{
    LogError(ex);

    // Reviewer: Use 'throw;' to preserve stack trace for debugging and middleware.
    throw; // Best practice

    // throw ex; // BAD: Loses stack trace, harder to debug
}

Why?
Preserving the stack trace helps you find the real source of the error.
If you use throw ex;, you lose this information, making production debugging much harder.
What if not used?

You’ll struggle to diagnose root causes in logs and error reports.


**8. Real-Time System Design: With vs. Without Exception Handling**
With:

Users get clear, actionable error messages
System logs all attempts
No negative balances or invalid operations
Developers can debug issues quickly

Without:

Crashes, negative balances, poor UX
Logs are incomplete or misleading
Hard to maintain, risky for business


**9. Best Practices for Experienced Developers**

Catch specific exceptions first
Never swallow exceptions
Use custom exceptions for domain errors
Rethrow with throw; to preserve stack trace
Centralize error handling with middleware


**10. Running the Demo**

Clone repo, open in Visual Studio/VS Code
Build and run (dotnet run)
Test /api/account/withdraw with various amounts and account IDs
Observe logs and error messages


**11. Code Review Highlights**

Added try-catch blocks for user-friendly errors
Used finally for guaranteed logging
Threw built-in and custom exceptions for clarity
Rethrew exceptions for global handling
Created InsufficientFundsException and InvalidAccountException for domain logic
Added middleware for centralized error handling
Explained throw vs rethrow with code and comments

**🚦12. Validation vs. Exception Handling in API Controllers
Validation**

Checks user input against business rules before processing.
Returns clear, actionable error messages to the client (e.g., BadRequest("Amount must be positive.")).
Should be handled with simple if statements or data annotations.

**Exception Handling**

Catches and manages truly unexpected errors (e.g., system failures, null references, database issues).
Uses try-catch blocks to log, respond, or recover from these errors.
Returns generic or internal error messages (e.g., StatusCode(500, "An unexpected error occurred.")).


**Examples**

Validation:
if (amount <= 0)
    return BadRequest("Amount must be positive.");


Exception Handling:
try
{
    // DB access or other risky operations
}
catch (Exception ex)
{
    // Log and return generic error
    return StatusCode(500, "An unexpected error occurred.");
}



**Why This Matters**

**Efficiency: ** Validation is fast; exceptions are expensive.
**Clarity:** Users get precise feedback for their mistakes.
**Maintainability:** Code is easier to read, test, and debug.

| Context           | Validation (if/return) | Exception (try/catch) |
|-------------------|-----------------------|-----------------------|
| User Input        | ✅                     | ❌                   |
| System Errors     | ❌                     | ✅                   |
| Business Rules    | ✅                     | ❌                   |
| Unpredictable     | ❌                     | ✅                   |


**In summary:**

Use validation for all predictable, user-facing issues.
Use exception handling for rare, unpredictable system failures.


-----------------------------------------------------------------------------------------
**Exception Handling & Validation Pattern – Q&A**
-----------------------------------------------------------------------------------------

**1. Where should input validation happen?**
Q: Where do I validate user input in a typical layered .NET application?
A:

**Controllers**: Validate user input at the boundary using utilities like Verify or data annotations.
**Services**: Validate business rules and contract requirements. Throw exceptions if these are violated.


**2. Where should exceptions be caught and handled?**
Q: Should I use try-catch in the service/manager layer, the controller, or both?
A:

**Services**: Use try-catch only if you can recover from the error (e.g., retry, fallback) or add useful context (e.g., wrap with business info or log and rethrow).
**Controllers**: Catch known exceptions and map them to user-friendly HTTP responses (e.g., BadRequest, Conflict). Let unexpected exceptions bubble up to global middleware.
**Mappers/Helpers**: Let exceptions bubble up unless you need to add context.


**3. What does “recover or add context” mean in exception handling?**
Q: What does it mean for the service layer to only catch exceptions if it can recover or add context?
A:

**Recover**: The service can handle the exception so the operation can succeed or degrade gracefully (e.g., use cached data if a remote call fails).
**Add Context:** The service catches an exception to provide more diagnostic information (e.g., wrap a low-level exception with business-specific details).


**4. Can you show examples of each scenario?**
A:
**A. Recovering from an Exception**

public AccountBalance GetAccountBalance(string accountId)
{
    try
    {
        return _remoteBankApi.GetBalance(accountId);
    }
    catch (TimeoutException)
    {
        _logger.LogWarning("Remote API timed out. Using cached balance.");
        return _cache.GetBalance(accountId);
    }
}

Here, the service “recovers” by providing a fallback, so the controller never sees the error.

**B. Adding Context to an Exception**

public void Withdraw(string accountId, decimal amount)
{
    try
    {
        _repository.Withdraw(accountId, amount);
    }
    catch (SqlException ex)
    {
        throw new DataAccessException($"Failed to withdraw from account {accountId}.", ex);
    }
}

Here, the service “adds context” by wrapping the original exception with more details for easier troubleshooting.

**C. Letting Exceptions Bubble Up**

public void Withdraw(string accountId, decimal amount)
{
    _repository.Withdraw(accountId, amount);
}

If you can’t recover or add context, let the exception bubble up to the controller or middleware.

**5. What are the pros and cons of each pattern?**
Q: What are the trade-offs between handling exceptions in the service layer, controller, or both?
A:

| Layer                     | Validation (Verify) | Try-Catch/Logging        | Exception Handling                |
|---------------            |---------------------|-------------------------|-----------------------------------|
| Service/Manager Layer Only| ❌                  | ❌ (unless context)      | Let exceptions bubble up          |
| Controller Layer Only     | ✅                  | ✅ (log/rethrow)         | Add context if needed         |
| Controller                | ✅                  | ✅ (map to HTTP)         | User-friendly error output        |

Pattern
Pros
Cons




Service/Manager Layer Only
Centralized logging, cleaner controllers, global error handling
Less granular control, risk of generic error responses


Controller Layer Only
Fine-grained HTTP mapping, separation of concerns, easier testing
Potentially repetitive code, possible missed logging


Both Layers
Detailed logging and precise error mapping, clear responsibilities
More boilerplate, risk of accidentally swallowing exceptions




6. Which pattern should I follow?
Q: How do I decide which approach to use?
A:

Use service/manager layer only if you have robust global exception middleware and want minimal controllers.
Use controller layer if you want fine-grained control over HTTP responses and more specific client feedback.
Use both for maximum robustness in complex applications.


7. What’s the summary of best practices?
A:

Validate input in controllers.
Log and rethrow in service/manager layer only if you need to add context or diagnose issues.
Map exceptions to HTTP responses in controllers, unless you have comprehensive global error handling middleware.
Let mappers/helpers bubble exceptions up.
Always log exceptions at the point of failure, especially for business logic errors.


8. Is there a quick reference table?

A:

| Layer         | Validation (Verify) | Try-Catch/Logging        | Exception Handling                |
|---------------|---------------------|-------------------------|-----------------------------------|
| Mapper        | ❌                  | ❌ (unless context)      | Let exceptions bubble up          |
| Service       | ✅                  | ✅ (log/rethrow)         | Add context if needed             |
| Controller    | ✅                  | ✅ (map to HTTP)         | User-friendly error output        |
| Middleware    | ❌                  | ✅ (global catch)        | Generic error output              |

9. What does “recover or add context” look like in practice?

A:

| Scenario                  | Should Service Catch? | What to Do                  |
|---------------------------|----------------------|-----------------------------|
| Can recover (fallback)    | Yes                  | Handle and return/fallback  |
| Can add context           | Yes                  | Wrap/log and rethrow        |
| Neither                   | No                   | Let exception bubble up     |

10. What’s the key takeaway?
A:
Catch exceptions in the service/manager layer only if you can actually handle the error (recover) or make it more useful (add context). Otherwise, let the exception bubble up to the controller or global middleware for consistent and user-friendly error handling.
