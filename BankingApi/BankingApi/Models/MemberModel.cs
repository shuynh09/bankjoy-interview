using System.Collections.Generic;

namespace BankingApi.Models
{
    public class MemberModel
    {
        public int MemberId { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public int InstitutionId { get; set; }
        public List<Account> Accounts { get; set; }
    }

    public class Account
    {
        public int AccountId { get; set; }
        public double Balance { get; set; }
    }
}
