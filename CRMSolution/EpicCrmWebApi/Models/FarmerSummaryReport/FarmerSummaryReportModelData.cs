using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class FarmerSummaryReportModelData : FarmerSummaryReportModel
    {
            public decimal RatePerKg { get; set; }
            public string HQName { get; set; }
            public string TerritoryName { get; set; }
            public decimal LandInSize { get; set; }
            public DateTime Issuedate { get; set; }
            public string IssueSlipNumber { get; set; }
            public string IssueInput { get; set; }
            public decimal IssueQuantity { get; set; }
            public string Uom { get; set; }
            public decimal PricePerUom { get; set; }
            public decimal InputAmount { get; set; }
            public string STRNumber { get; set; }
            public string DWSNumber { get; set; }
            public DateTime PurchaseDate { get; set; }
            public decimal DWSQuantity { get; set; }
            public decimal PurchaseAmount { get; set; }
            public string PaymentReference { get; set; }
            public decimal DWSDeduction { get; set; }
            public decimal Netpayable { get; set; }
            public DateTime PayoutDate { get; set; }
            public decimal PaymentAmount { get; set; }
            public DateTime AdvanceRequestDate { get; set; }
            public decimal AmountApproved { get; set; }
            public string FarmerId { get; set; }

            public string IssueType { get; set; }
           public decimal TotalNetQuantity { get; set; }
           public decimal BonusRate { get; set; }
           public decimal BonusAmountPaid { get; set; }
           public decimal BonusPayableAmount { get; set; }
           public DateTime PaymentDate { get; set; }
           public string BonusPaymentReference { get; set; }
    }
}
