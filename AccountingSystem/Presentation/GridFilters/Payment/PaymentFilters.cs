using System;

namespace Presentation.Extensions.Payment
{
    public class PaymentFilters
    {
        public string[] forder { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string paymentReferenceNo { get; set; }
        public string paymentDate { get; set; }
        public string paymentStatus { get; set; }
        public string createdBy { get; set; }
        public string approvedBy { get; set; }
        public decimal payableAmount { get; set; }
        public decimal payableMaxAmount { get; set; }
        public decimal pay { get; set; }
    }
}
