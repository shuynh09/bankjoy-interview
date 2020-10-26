namespace BankingApi.Models
{
    public class TransferModel
    {
        public int MemberId { get; set; }
        public int AccountId { get; set; }
        public double TransferAmount { get; set; }
        public int TransferMemberId { get; set; }
        public int TransferAccountId { get; set; }
    }
}
