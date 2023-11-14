using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.DTOs.Payment
{
    public class BillpaymentDto
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


    }
}
