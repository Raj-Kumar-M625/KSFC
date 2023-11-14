using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class DownloadAgreementModel
    {
        public string SeasonName { get; set; }
        public string TypeName { get; set; }  // crop name goes here
        public long AgreementId { get; set; }
        public string AgreementNumber { get; set; }
        public string Status { get; set; }
        public decimal LandSizeInAcres { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public ICollection<DownloadIssueReturnModel> IssueReturns { get; set; }
        public ICollection<DownloadAdvanceRequestModel> AdvanceRequests { get; set; }
        public ICollection<DownloadDWSModel> DWS { get; set; }
    }

    public class DownloadSurveyModel
    {
        public string SeasonName { get; set; }
        public string SowingType { get; set; }  // crop name goes here
        public long SurveyId { get; set; }
        public string SurveyNumber { get; set; }
        public string Status { get; set; }
        public decimal LandSizeInAcres { get; set; }
        public string SowingDate { get; set; }
    }

    public class DownloadIssueReturnModel
    {
        public long Id { get; set; }
        public string TransactionDate { get; set; }
        public string SlipNumber { get; set; }
        public string TransactionType { get; set; }
        public long ItemMasterId { get; set; }
        public string ItemType { get; set; }
        public string ItemCode { get; set; }
        public string ItemUnit { get; set; }
        public int Quantity { get; set; }
        public decimal ItemRate { get; set; }
        public string Status { get; set; }
        public bool IsIssueItem { get; set; }
        public bool IsApproved { get; set; }
    }

    public class DownloadAdvanceRequestModel
    {
        public long Id { get; set; }
        public string AdvanceRequestDate { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public bool IsApproved { get; set; }
    }

    public class DownloadDWSModel
    {
        public long Id { get; set; }
        public string DWSNumber { get; set; }
        public string DWSDate { get; set; }
        public long BagCount { get; set; }
        public decimal FilledBagsWeightKg { get; set; }
        public decimal EmptyBagsWeightKg { get; set; }
        public decimal GoodsWeight { get; set; }

        public decimal SiloDeductWt { get; set; }
        public decimal NetPayableWt { get; set; }
        public decimal RatePerKg { get; set; }

        public decimal GoodsPrice { get; set; }
        public decimal DeductAmount { get; set; }
        public decimal NetPayable { get; set; }
        public string Status { get; set; }
        public string BankAccountName { get; set; }
        public string BankName { get; set; }
        public string PaidDate { get; set; }

        public bool IsPending { get; set; }
        public bool IsWeightApproved { get; set; }
        public bool IsAmountApproved { get; set; }
        public bool IsPaid { get; set; }
    }
}