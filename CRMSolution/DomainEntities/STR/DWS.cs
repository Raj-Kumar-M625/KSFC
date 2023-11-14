using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class DWS
    {
        public long Id { get; set; }
        public long STRTagId { get; set; }
        public long STRId { get; set; }
        public string DWSNumber { get; set; }
        public System.DateTime DWSDate { get; set; }
        public long BagCount { get; set; }
        public decimal FilledBagsWeightKg { get; set; }
        public decimal EmptyBagsWeightKg { get; set; }
        public long EntityId { get; set; }
        public string EntityName { get; set; }
        public long AgreementId { get; set; }
        public string Agreement { get; set; }
        public long EntityWorkFlowDetailId { get; set; }
        public string TypeName { get; set; }
        public string TagName { get; set; }
        public long ActivityId { get; set; }

        public string HQCode { get; set; }

        public decimal SiloDeductPercent { get; set; }
        public decimal GoodsWeight { get; set; }
        public decimal SiloDeductWt { get; set; }
        public decimal SiloDeductWtOverride { get; set; }
        public decimal NetPayableWt { get; set; }
        public decimal RatePerKg { get; set; }
        public decimal GoodsPrice { get; set; }
        public decimal DeductAmount { get; set; }
        public decimal NetPayable { get; set; }
        public long OrigBagCount { get; set; }
        public decimal OrigFilledBagsKg { get; set; }
        public decimal OrigEmptyBagsKg { get; set; }
        public string Comments { get; set; }
        public string WtApprovedBy { get; set; }
        public Nullable<System.DateTime> WtApprovedDate { get; set; }
        public string AmountApprovedBy { get; set; }
        public Nullable<System.DateTime> AmountApprovedDate { get; set; }
        public string PaidBy { get; set; }
        public Nullable<System.DateTime> PaidDate { get; set; }
        public string PaymentReference { get; set; }
        public string Status { get; set; }
        public long CyclicCount { get; set; }

        public string BankAccountName { get; set; }
        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public string BankIFSC { get; set; }
        public string BankBranch { get; set; }

        public bool IsPendingStatus => Status.Equals(STRDWSStatus.Pending.ToString(), StringComparison.OrdinalIgnoreCase);
        public bool IsApproveWeightAllowed => Status.Equals(STRDWSStatus.SiloChecked.ToString(), StringComparison.OrdinalIgnoreCase);
        public bool IsApproveAmountAllowed => Status.Equals(STRDWSStatus.WeightApproved.ToString(), StringComparison.OrdinalIgnoreCase);
        public bool IsReadyToPay => Status.Equals(STRDWSStatus.AmountApproved.ToString(), StringComparison.OrdinalIgnoreCase);


        public long StrTagCyclicCount { get; set; }

        public string CurrentUser { get; set; }
        public string STRNumber { get; set; }
        public long EmployeeId { get; set; }
        public long MoveToStrTagId { get; set; }

        public bool ChangeSTRNumber { get; set; }
        public bool ChangeDWSData { get; set; }

        public bool ChangeAgreementDetails { get; set; } = false;
    }

    public class DWSAudit : DWS
    {
        public long DWSId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
