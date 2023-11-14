using Application.DTOs.Vendor;
using Domain.Payment;
using Domain.Vendor;
using System;

namespace Application.DTOs.Payment
{
    public class MappingAdvancePaymentDto
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        public int PaymentsID { get; set; }
        public int AdvancePaymentId { get; set; }
        public decimal? AdvanceAmount { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        //public virtual BillItems BillItems { get; set; }
        public virtual PaymentDto Payments { get; set; }
        public virtual VendorDetailsDto Vendor { get; set; }
    }
}
