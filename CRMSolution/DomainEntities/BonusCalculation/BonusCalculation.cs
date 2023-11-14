using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class BonusCalculation
    {
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime AgreementDate { get; set; }
        public string AgreementNumber { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string TypeName { get; set; }
        public string EntityName { get; set; }
        public long EntityId { get; set; }
        public decimal LandSizeInAcres { get; set; }
        public decimal RatePerKg { get; set; }
        public decimal NetPayableWt { get; set; }
        public decimal NetPayable { get; set; }
        public decimal BonusRate { get; set; }
        public decimal YeildPerAcre { get; set; }
        public string AccountHolderName { get; set; }
        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public string BankIFSC { get; set; }
        public string BankBranch { get; set; }
        public long SeasonId { get; set; }
        public string SeasonName { get; set; }
        public string HQCode { get; set; }
        public string AgreementStatus { get; set; }
        public string DWSStatus { get; set; }
        public string BonusStatus { get; set; }
        public decimal BonusAmountPayable { get; set; }
        public decimal BonusAmountPaid { get; set; }
        public string Comments { get; set; }
        public long BankId { get; set; }
        public long AgreementId { get; set; }
        public string PaymentReference { get; set; }
        public bool IsEditAllowed => String.IsNullOrEmpty(BonusStatus) || BonusStatus.Equals("Pending") || BonusStatus.Equals("AwaitingApproval");

    }
}
