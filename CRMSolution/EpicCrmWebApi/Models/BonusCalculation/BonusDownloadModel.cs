using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class BonusDownloadModel
    {
        [Display(Name = "Farmer Name")]
        public string EntityName { get; set; }

        [Display(Name = "Agreement Date")]
        public DateTime AgreementDate { get; set; }

        [Display(Name = "Payment Date")]
        public DateTime PaymentDate { get; set; }

        [Display(Name = "Agreement Number")]
        public string AgreementNumber { get; set; }

        [Display(Name = "Crop")]
        public string TypeName { get; set; }

        [Display(Name = "Season Name")]
        public string SeasonName { get; set; }

        [Display(Name = "Approved Weight")]
        public decimal NetWeight { get; set; }

        [Display(Name = "Payment Type")]
        public string PaymentType { get; set; }

        [Display(Name = "Notes")]
        public string Comments { get; set; }

        [Display(Name = "IFSC CODE")]
        public string BankIFSC { get; set; }

        [Display(Name = "Bonus Amount")]
        public decimal BonusPaid { get; set; }

        [Display(Name = "Bonus Rate")]
        public decimal BonusRate { get; set; }

        [Display(Name = "REMITTER'S A/C NO.")]
        public string RemitterAccount { get; set; }

        [Display(Name = "REMITTER'S NAME")]
        public string RemitterName { get; set; }

        [Display(Name = "REMITTER'S ADD.")]
        public string RemitterAddress { get; set; }

        //[Display(Name = "BENEFICIARY A/C NO.")]
        //public string BankAccount { get; set; }

        //[Display(Name = "BENEFICIARY NAME")]
        //public string BankAccountName { get; set; }

        [Display(Name = "REMITTER'S BANK NAME")]
        public string BankName { get; set; }

        //[Display(Name = "BRANCH")]
        //public string BankBranch { get; set; }

        [Display(Name = "Payment Reference Number")]
        public string PaymentReference { get; set; }

        [Display(Name = "Sender Information")]
        public string SenderInfo { get; set; }

        [Display(Name = "BO Mail Id")]
        public string RemitterEmail { get; set; }
    }
}