using Presentation.Models.Master;
using Presentation.Models.Transactions;
using System;

namespace Presentation.Models.Bank
{
    public class ReconciliationModel
    {
        public int Id { get; set; }
        public int BankTransactionsId { get; set; }
        public int TransactionsId { get; set; }
        public string SystemName { get; set; }
        public decimal Amount { get; set; }
        public DateTime? ReconciledDate { get; set; }
        public DateTime TransactionDate { get; set; }
        public string ReconcileStatus { get; set; }
        public DateTime MatchedDate { get; set; }
        public string MatchedBy { get; set; }
        public bool IsActive { get; set; }
        public decimal? ClosingBalance { get; set; }
        public string Description { get; set; }
        public string ChargeOrPayment { get; set; }
        public string TransactionDetailedType { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public virtual BankTransactionModel BankTransactions { get; set; }
        public virtual TransactionsSummaryModel TransactionsSummary { get; set; }
       // public virtual CommonMasterModel StatusMaster { get; set; }
    }
}
