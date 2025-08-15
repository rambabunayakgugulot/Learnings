namespace ExceptionHandlingInBankingAPI.Models
{
    public class WithdrawalRequest
    {
        public decimal Amount { get; set; }
        public string AccountId { get; set; }
    }
}
