using Domain.Bill;
using Domain.Vendor;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Payment
{
    public class MappingAdvancePayment
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Vendor")]
        public int VendorId { get; set; }

        [ForeignKey("Payments")]
        public int? PaymentsID { get; set; }
        public int AdvancePaymentId { get; set; }
        public decimal? AdvanceAmount { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsActive { get; set; }
        //public virtual BillItems BillItems { get; set; }
        public virtual Payments Payments { get; set; }
        public virtual Vendors Vendor { get; set; }


    }
}
