using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;

namespace Presentation.Models.BankFile
{
    public class BankFileUtrDetailsModel
    {
        public int Id { get; set; }
        public int NoOfTransactions { get; set; }
        public string DifferentBankUTRNumber { get; set; }
        public string SameBankUTRNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifedBy { get; set; }
        public bool IsBulkPayment { get; set; }
        public List<int> GenerateBankFileID { get; set; }
    }
}
