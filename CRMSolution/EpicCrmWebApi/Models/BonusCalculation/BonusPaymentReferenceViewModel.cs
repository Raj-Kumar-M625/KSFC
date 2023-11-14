using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EpicCrmWebApi
{
    public class BonusPaymentReferenceViewModel
    {
        public long Id { get; set; }

        [Display(Name = "Payment Reference")]
        public string PaymentReference { get; set; }

        [Display(Name = "Agreement Number")]
        public string AgreementNumber { get; set; }

        [Display(Name = "Agreement Count")]
        public long AgreementCount { get; set; }

        [Display(Name = "Notes")]
        public string Comments { get; set; }

        [Display(Name = "Total Bonus Paid")]
        public decimal TotalBonusPaid { get; set; }

        [Display(Name = "Prepared By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Date Prepared")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Remitter Bank Name")]
        public string BankName { get; set; }

        [Display(Name = "Remitter A/c #")]
        public string AccountNumber { get; set; }

        [Display(Name = "Remitter's Name")]
        public string AccountName { get; set; }

        [Display(Name = "Remitter's Address")]
        public string AccountAddress { get; set; }

        [Display(Name = "Remitter's Email")]
        public string AccountEmail { get; set; }

        [Display(Name = "Payment Type")]
        public string PaymentType { get; set; }

        [Display(Name = "Sender/Receiver Info")]
        public string SenderInfo { get; set; }
    }
}