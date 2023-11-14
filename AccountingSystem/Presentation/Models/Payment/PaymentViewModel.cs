using Application.DTOs.Vendor;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.Payment
{
    public class PaymentViewModel
    {
        [Key]
        public int ID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string PaymentReferenceNo { get; set; }
        [MaxLength(50)]
        [Required]
        public string PaymentBillReference { get; set; }

        [ForeignKey("Vendor")]
        public int VendorID { get; set; }
        public decimal PaymentAmount { get; set; }
        [Required]
        public decimal PaidAmount { get; set; }
        public decimal AdvanceAmountUsed { get; set; }

        public decimal BalanceAdvanceAmount { get; set; }     
        public string ApprovedBy { get; set; }
        public DateTime PaymentDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        [MaxLength(150)]
        [Required]
        public string Description { get; set; } 
        public virtual VendorDetailsDto Vendor { get; set; }

        //public virtual Bills Bills { get; set; }

    }
}
