using Domain.Bill;
using Domain.Vendor;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Payment
{
    public class BillPayment
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Vendor")]
        public int VendorId { get; set; }
        [ForeignKey("Payments")]
        public int PaymentID { get; set; }
        [ForeignKey("Bill")]
        public int BillID { get; set; }
        public decimal BillAmount { get; set; }
        public decimal? PaymentAmount { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifedBy { get; set; }
        public bool IsActive { get; set; }
        public virtual Bills Bill { get; set; }
        [NotMapped]
        public decimal AdvancePaymentUsed { get; set; }
        [NotMapped]
        public string AdvancePayments { get; set; }

        [NotMapped]
        public decimal PaidAmount { get; set; }

        //public virtual BillItems BillItems { get; set; }
        public virtual Payments Payments { get; set; }
        public virtual Vendors Vendor { get; set; }


    }
}
