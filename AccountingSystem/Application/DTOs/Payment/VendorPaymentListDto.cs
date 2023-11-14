using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Payment
{
    public class VendorPaymentListDto
    {
        public string PaymentReferenceNo { get; set; }
        public string PayDate { get; set; }
        public string VendorName { get; set; }
        public decimal Paid { get; set; }
        public string CreatedBy { get; set; }
        public string ApprovedBy { get; set; }
        public string PaymentStatus { get; set; }
       
    }
}
