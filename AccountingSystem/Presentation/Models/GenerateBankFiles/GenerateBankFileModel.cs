using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models.GenerateBankFiles
{/// <summary>
 /// Author:Swetha M Date:04/11/2022
 /// Purpose:Generate Bnak File Mapping Table
 /// </summary>
 /// <returns></returns>
    public class GenerateBankFileModel
    {
        [Key]
        public int Id { get; set; }
        public string BankFileReferenceNo { get; set; }
        public int AccountNo { get; set; }
        public int BankMasterId { get; set; }
        public int NoOfVendors { get; set; }
        public int NoOfTransactions { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsBulkPayment { get; set; }
        public bool IsBulkPaymentModified { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string IFSCCode { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifedBy { get; set; }
        public List<int> PaymentId { get; set; }

    }
}
