using System;

namespace Presentation.GridFilters.TDS
{
    public class TdsFilter
    {
        public string[] forder { get; set; }
        public string vendorName { get; set; }
        public string paymentReferenceNo { get; set; }
        public decimal payableAmount { get; set; }
        public decimal payableMaxAmount { get; set; }
        public string paymentFromDate { get; set; }
        public DateTime paymentToDate { get; set; }
        public string tdsSection { get; set; }
        public string tdsStatus { get; set; }
        public int assessmentYear { get; set; }
        public int transactionType { get; set; }

    }
}
