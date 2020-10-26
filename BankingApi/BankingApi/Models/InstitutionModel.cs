using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BankingApi.Models
{
    public class InstitutionModel
    {
        public List<Institution> Institutions { get; set; }
        public List<MemberModel> Members { get; set; }
    }

    public class Institution
    {
        public int InstitutionId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
