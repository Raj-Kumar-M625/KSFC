using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class DownloadEntity
    {
        public long Id { get; set; }
        public string EntityType { get; set; }
        public string EntityName { get; set; }
        public string FatherHusbandName { get; set; }
        public string VillageName { get; set; }
        public string UniqueIdType { get; set; }
        public string UniqueId { get; set; }
        // added March 2020, while doing PJM Farming changes
        public string EntityNumber { get; set; } 
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public virtual ICollection<DownloadEntityContact> Contacts { get; set; }
        public virtual ICollection<DownloadEntityAgreement> Agreements { get; set; }
        public virtual ICollection<DownloadEntitySurvey> Surveys { get; set; }
        public virtual ICollection<DownloadEntityBankDetail> BankDetails { get; set; }
    }

    public class DownloadEntityAgreement
    {
        public string SeasonName { get; set; }
        public string TypeName { get; set; }  // crop name goes here
        public long AgreementId { get; set; }
        public string AgreementNumber { get; set; }
        public string Status { get; set; }
        public decimal LandSizeInAcres { get; set; }
        public virtual ICollection<DownloadIssueReturn> IssueReturns { get; set; }
        public virtual ICollection<DownloadAdvanceRequest> AdvanceRequests { get; set; }
        public virtual ICollection<DownloadDWS> DWS { get; set; }
    }

    public class DownloadEntitySurvey
    {
        public string SeasonName { get; set; }
        public string TypeName { get; set; }  // crop name goes here
        public long SurveyId { get; set; }
        public string SurveyNumber { get; set; }
        public string Status { get; set; }
        public decimal LandSizeInAcres { get; set; }
        public DateTime SowingDate { get; set; }
    }

    public class DownloadEntityBankDetail
    {
        public bool IsApproved { get; set; }
        public string Status { get; set; }
        public bool IsSelf { get; set; }
        public string AccountNumber { get; set; }
        public string AccountHolderName { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string Comments { get; set; }
    }

    //public class DownloadSeasonEntity : DownloadEntity
    //{
    //    public string SeasonName { get; set; }
    //    public string TypeName { get; set; }  // crop name goes here
    //    public long AgreementId { get; set; }
    //    public string AgreementNumber { get; set; }
    //    public bool IsAgreementApproved { get; set; }
    //    public bool IsAgreementPending { get; set; }
    //    public bool IsAgreementTerminated { get; set; }
    //}

    public class DownloadEntityContact
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsPrimary { get; set; }
    }

    public class DownloadIssueReturn
    {
        public long Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string SlipNumber { get; set; }

        public string TransactionType { get; set; }
        public long ItemMasterId { get; set; }
        public string ItemType { get; set; }
        public string ItemCode { get; set; }
        public string ItemUnit { get; set; }
        public int Quantity { get; set; }
        public decimal ItemRate { get; set; }


        public string AppliedTransactionType { get; set; }
        public long AppliedItemMasterId { get; set; }
        public string AppliedItemType { get; set; }
        public string AppliedItemCode { get; set; }
        public string AppliedItemUnit { get; set; }
        public int AppliedQuantity { get; set; }
        public decimal AppliedItemRate { get; set; }


        public string Status { get; set; }

        public bool IsIssueItem
        {
            get
            {
                if (String.IsNullOrEmpty(AppliedTransactionType))
                {
                    return false;
                }

                return AppliedTransactionType.Contains("Issue");
            }
        }

        public bool IsApproved => "Approved".Equals(Status, StringComparison.OrdinalIgnoreCase);
    }

    public class DownloadAdvanceRequest
    {
        public long Id { get; set; }
        public System.DateTime AdvanceRequestDate { get; set; }
        public string Status { get; set; }
        public decimal AmountRequested { get; set; }
        public decimal AmountApproved { get; set; }

        public bool IsApproved => Status.Contains("Approved");
    }

    public class DownloadDWS
    {
        public long Id { get; set; }
        public string DWSNumber { get; set; }
        public System.DateTime DWSDate { get; set; }
        public long BagCount { get; set; }
        public decimal FilledBagsWeightKg { get; set; }
        public decimal EmptyBagsWeightKg { get; set; }
        public decimal GoodsWeight { get; set; }
        public decimal SiloDeductWt { get; set; }
        public decimal SiloDeductWtOverride { get; set; }
        public decimal NetPayableWt { get; set; }
        public decimal RatePerKg { get; set; }
        public decimal GoodsPrice { get; set; }
        public decimal DeductAmount { get; set; }
        public decimal NetPayable { get; set; }
        public string Status { get; set; }

        public string BankAccountName { get; set; }
        public string BankName { get; set; }
        public DateTime? PaidDate { get; set; }

        public bool IsPending => Status.Equals(STRDWSStatus.Pending.ToString(), StringComparison.OrdinalIgnoreCase) ||
            Status.Equals(STRDWSStatus.SiloChecked.ToString(), StringComparison.OrdinalIgnoreCase) ||
            Status.Equals(STRDWSStatus.Approved.ToString(), StringComparison.OrdinalIgnoreCase);

        public bool IsWeightApproved => Status.Equals(STRDWSStatus.WeightApproved.ToString(), StringComparison.OrdinalIgnoreCase);

        public bool IsAmountApproved => Status.Equals(STRDWSStatus.AmountApproved.ToString(), StringComparison.OrdinalIgnoreCase);

        public bool IsPaid => Status.Equals(STRDWSStatus.Paid.ToString(), StringComparison.OrdinalIgnoreCase);
    }
}
