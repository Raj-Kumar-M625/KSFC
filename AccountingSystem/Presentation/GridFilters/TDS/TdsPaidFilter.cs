using System;

namespace Presentation.GridFilters.TDS
{
    public class TdsPaidFilter
    {
        public string[] forder { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public string vendorName { get; set; }
        public string paymentReferenceNo { get; set; }
        public string tdsSection { get; set; }
        public string tdsStatus { get; set; }
        public string challanNo { get; set; }
        public string utrNo { get; set; }
        public string bsrCode { get; set; }
        public decimal payableMinAmount { get; set; }
        public decimal payableMaxAmount { get; set; }
    }
}
