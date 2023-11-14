using Domain.Bill;
using Domain.Vendor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Payment
{
    public class Payments
    {
        [Key]
        public int ID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string PaymentReferenceNo { get; set; }
        public string PaymentBillReference { get; set; }

        [ForeignKey("Vendor")]
        public int VendorID { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]

        public decimal PaymentAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]

        public decimal PaidAmount { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime PaymentDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public decimal PaymentAmountAgainstOB { get; set; }
        public string Remarks { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public decimal AdvanceAmountUsed { get; set; }
        public decimal BalanceAdvanceAmount { get; set; }
        [NotMapped]
        public decimal BalanceAmount { get; set; }

        public virtual Vendors Vendor { get; set; }

        public virtual PaymentStatus PaymentStatus { get; set; }
        public List<MappingAdvancePayment> MappingAdvancePayment { get; set; }
       // public virtual MappingAdvancePayment MappingAdvancePayment { get; set; }
        // public virtual Bills Bills { get; set; }
    }
}
