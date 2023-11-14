using System;

namespace Application.DTOs.Vendor
{
    public class VendorListDto
    {
        public string Name { get; set; }
        public string GSTIN_Number { get; set; }
        public string PAN_Number { get; set; }
        public string TAN_Number { get; set; }
        public string Category { get; set; }
        public decimal? OpeningBalance { get; set; }
        public decimal? TotalBillAmount { get; set; }
        public decimal? TotalNetPayable { get; set; }
        public decimal? AmountPaid { get; set; }
        public decimal? BalanceAmount { get; set; }
        public decimal? TDSPercentage { get; set; }
        public DateTime? TDSPaidDate { get; set; }
        public DateTime? TDSFiledDate { get; set; }
        public decimal? GST_TDSPercentage { get; set; }
        public DateTime? GST_TDSPaidDate { get; set; }
        public DateTime? GST_TDSFiledDate { get; set; }
        public string Status { get; set; }


    }
}
