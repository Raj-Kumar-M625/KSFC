using System.ComponentModel.DataAnnotations.Schema;

namespace EDCS_TG.API.Data.Models
{
    public class Employment:BaseEntity
    {
        [ForeignKey("PersonalDetails")] public int PersonalDetailsId { get; set; }
        public string? EmploymentStatus { get; set; }
         public string? EmploymentStatusIfothers { get; set; } 
         public string? NatureOfEmploymentOthers { get; set; }

        public string Wheredoyouwork { get; set; }
         public string Income { get; set; }

        public string? OthersSpecify { get; set; }
        public string? PrimarySourceofIncome { get; set; }

        public string? SecondarySourceWork { get; set; }
        public string? SecondarySourceofIncome { get; set; }
        public string AnnualIncome { get; set; }
        public string HavePositionofOrganization { get; set; }
        public bool? PositionofOrganization { get; set; }
         public string? EmploymentCardMembership { get; set; }  
        public string ? EmploymentCardMembershipOthers { get; set; }
        public string? RegisteredEmploymentAgency { get; set; }
        public bool RegisteredEmploymentAgencyWhich { get; set; } 
        public string MoneySavingInvestments { get; set; }    
        public bool? Loans { get; set; }
        public bool? ExistingLoans { get; set; }
        public bool? Accounts { get; set; }
        public int? AccountsOthers { get; set; }






    }
}
