namespace BankingApi.Models
{
    public class BalanceModel
    {
        public int MemberId { get; set; }
        public int AccountId { get; set; }
        public double Balance { get; set; }
    }
}
