using Domain.Master;
using Domain.Payment;
using Domain.Vendor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Transactions
{
    public class Transaction
    {
        public int ID { get; set; }
        public int ReferenceId { get; set; }

        //[ForeignKey(nameof(Vendor))]
        public int? VendorID { get; set; }
        public string TransactionType { get; set; }
        public string VendorName { get; set; }
        public string PhoneNumber { get; set; }
        public int? ReferenceNumber { get; set; }
        public string ReferenceType { get; set; }

        public string GSTIN_Number { get; set; }
        public string PAN_Number { get; set; }
        public string TAN_Number { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string IFSCCode { get; set; }
        public string UTRNumber { get; set; }
        public string BillReferenceNo { get; set; }
        public string BillNo { get; set; }
        public string PaymentReferenceNo { get; set; }
        public decimal Amount { get; set; }
        public string Cheque_No { get; set; }
        public string ChallanNo { get; set; }
        public string Description { get; set; }
        public string Scheme { get; set; }
        public string AssesmentYear { get; set; }
        public string FinancialYear { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime? TransactionGeneratedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string TransactionDetailedType { get; set; }
        public string Status { get; set; }
        public string ReconcileStatus { get; set; }
        public string TransactionRefNo { get; set; }

        [NotMapped]
        public string CodeValue { get; set; }
        [NotMapped]

        public string Type { get; set; }
        [NotMapped]
        public string ApprovedBy { get; set; }
        [NotMapped]

        public decimal AdvanceAmountUsed { get; set; }



    }
}
