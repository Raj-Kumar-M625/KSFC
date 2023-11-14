using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EpicCrmWebApi
{
    public class VendorPaymentReferenceViewModel
    {
        public long Id { get; set; }

        [Display(Name = "Payment Reference")]
        public string PaymentReference { get; set; }

        [Display(Name = "STR Number")]
        public string STRNumber { get; set; }

        [Display(Name = "STR Count")]
        public long STRCount { get; set; }

        [Display(Name = "Notes")]
        public string Comments { get; set; }

        [Display(Name = "Total Amount")]
        public decimal TotalNetPayable { get; set; }

        
        [Display(Name = "Prepared By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Date Prepared")]
        public System.DateTime DateCreated { get; set; }

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