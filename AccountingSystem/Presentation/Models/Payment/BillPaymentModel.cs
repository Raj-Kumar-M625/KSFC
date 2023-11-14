using Domain.Bill;
using Domain.Payment;
using Presentation.Models.Vendor;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.Payment
{
    public class BillPaymentModel
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

        public virtual BillItems BillItems { get; set; }
        public virtual Payments Payments { get; set; }
        public virtual VendorViewModel Vendor { get; set; }
        public virtual PaymentStatus PaymentStatus { get; set; }
        public virtual VendorPersonModel VendorPerson { get; set; } /*= new VendorPersonModel();*/

    }
}
