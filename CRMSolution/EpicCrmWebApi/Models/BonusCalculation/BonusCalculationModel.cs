using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace EpicCrmWebApi
{
    public class BonusCalculationModel
    {
        public System.DateTime DateCreated { get; set; }

        [Display(Name = "Agreement Number")]
        public string AgreementNumber { get; set; }
        public long AgreementId { get; set; }

        [Display(Name = "Crop Name")]
        public string TypeName { get; set; }

        [Display(Name = "Farmer Name")]
        public string EntityName { get; set; }

        [Display(Name = "Entity Id")]
        public long EntityId { get; set; }

        [Display(Name = "Total Acres")]
        public decimal LandSizeInAcres { get; set; }

        [Display(Name = "Rate Per Kg")]
        public decimal RatePerKg { get; set; }

        [Display(Name = "Approved Weight")]
        public decimal NetPayableWt { get; set; }

        [Display(Name = "Net Amount Paid")]
        public decimal NetPayable { get; set; }

        public long BankId { get; set; }

        [Display(Name = "Account Holder Name")]
        public string AccountHolderName { get; set; }

        [Display(Name = "Bank Name")]
        public string BankName { get; set; }

        [Display(Name = "Account Number")]
        public string BankAccount { get; set; }

        [Display(Name = "Bank IFSC")]
        public string BankIFSC { get; set; }

        [Display(Name = "Bank Branch")]
        public string BankBranch { get; set; }
        public bool IsEditAllowed { get; set; }
        public string HQCode { get; set; }
        public string BonusStatus { get; set; }

        [Display(Name = "Season Name")]
        public string SeasonName { get; set; }

        [Display(Name = "Yield Per Acre")]
        public decimal YieldPerAcre { get; set; }

        [Display(Name = "Bonus Rate")]
        public decimal BonusRate { get; set; }

        [Display(Name = "Bonus Amount Calculated")]
        public decimal BonusAmountPayable { get; set; }

        [Display(Name = "Bonus Amount Payable")]
        public decimal BonusAmountPaid { get; set; }

        [Display(Name = "Notes")]
        public string Comments { get; set; }

        [Display(Name = "Payment Reference")]
        public string PaymentReference { get; set; }

        [Display(Name = "Agreement Date")]
        public DateTime AgreementDate { get; set; }

        public ICollection<EntityBankDetailModel> EntityBankAccounts { get; set; }
    }
}