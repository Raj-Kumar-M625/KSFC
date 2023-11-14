using System;

namespace Presentation.Extensions.Bill
{
    public class BillFilter
    {
        public string vendorName { get; set; }
        public string ReferenceNo { get; set; }
        public string BillNumber { get; set; }
        public decimal BillTotal { get; set; }
        public Decimal TotalPayableAmount { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public string BillDate { get; set; }
        public string DueDate { get; set; }
        public string[] forder { get; set; }
    }
}
