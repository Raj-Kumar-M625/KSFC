using Application.DTOs.Vendor;
using Domain.Bill;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Payment
{
    public class PaymentDto
    {
        [Key]
        public int ID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string PaymentReferenceNo { get; set; }
        public string PaymentBillReference { get; set; }

        [ForeignKey("Vendor")]
        public int VendorID { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal PaidAmount { get; set; }      
        public string ApprovedBy { get; set; }
        public DateTime PaymentDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }

        public decimal PaymentAmountAgainstOB { get; set; }
        public decimal AdvanceAmountUsed { get; set; }
        public decimal BalanceAdvanceAmount { get; set; }
        public decimal AdvancePaymentUsed { get; set; }
        public string AdvancePayments { get; set; }

        public string Remarks { get; set; }
        public virtual PaymentStatusDto PaymentStatus { get; set; }

        public virtual VendorDetailsDto Vendor { get; set; }
        public List<MappingAdvancePaymentDto> MappingAdvancePayment { get; set; }
        public virtual Bills Bills { get; set; }

    }
}
