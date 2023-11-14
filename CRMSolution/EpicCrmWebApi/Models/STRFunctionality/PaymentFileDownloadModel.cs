using BusinessLayer;
using CRMUtilities;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class PaymentFileDownloadModel
    {
        [Display(Name = "SL NO")]
        public int SNo { get; set; }

        [Display(Name = "STR Number")]
        public string STRNumber { get; set; }

        [Display(Name = "IFSC CODE")]
        public string BankIFSC { get; set; }

        [Display(Name = "TRANSACTION AMT")]
        public decimal NetPayable { get; set; }

        [Display(Name = "COMMISSION AMT")]
        public decimal Commission
        {
            get
            {
                if (NetPayable <= 10000)
                {
                    return 2.36M;
                }

                if (NetPayable <= 100000)
                {
                    return 4.72M;
                }

                return 14.16M;
            }
        }

        [Display(Name = "AMOUNT")]
        public decimal Amount => NetPayable + Commission;

        [Display(Name = "REMITTER'S A/C NO.")]
        public string RemitterAccount { get; set; }

        [Display(Name = "REMITTER'S NAME")]
        public string RemitterName { get; set; }

        [Display(Name = "REMITTER'S ADD.")]
        public string RemitterAddress { get; set; }

        
        [Display(Name = "BENEFICIARY A/C NO.")]
        public string BankAccount { get; set; }

        [Display(Name = "BENEFICIARY NAME")]
        public string BankAccountName { get; set; }

        [Display(Name = "BANK NAME")]
        public string BankName { get; set; }

        [Display(Name = "BRANCH")]
        public string BankBranch { get; set; }

        [Display(Name = "PAYMENT DETAILS")]
        public string PaymentDetails { get; set; }

        [Display(Name = "SENDER TO RECEIVER INFORMATION")]
        public string SenderToReceiverInfo { get; set; }

        [Display(Name = "BO Mail Id")]
        public string RemitterEmail { get; set; }
    }
}