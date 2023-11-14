using System;

namespace Presentation.GridFilters.GSTTDS
{
    public class GstTdsPaidFilter
    {
        public string[] forder { get; set; }

        public string billReferenceNo { get; set; }

        public string vendorName { get; set; }

        public string paymentFromDate { get; set; }

        public DateTime paymentToDate { get; set; }

        public decimal payableMinAmount { get; set; }

        public decimal payableMaxAmount { get; set; }

        public string gstCertificate { get; set; }

        public string gstinNumber { get; set; }

        public string gstTdsStatus { get; set; }
        public string utrNo { get; set; }
    }
}
