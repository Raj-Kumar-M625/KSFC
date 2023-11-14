using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Vendor;
using Domain.Payment;

namespace Domain.Adjustment
{
    public class Adjustments
    {
        [Key]
        public int ID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string AdjustmentReferenceNo { get; set; }
        public string BillPaymentRefNo { get; set; }
        public DateTime Date { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal Amount { get; set; }
        [ForeignKey(nameof(Vendor))]
        public int VendorID { get; set; }
        public string Description { get; set; }
        public string UTR_No { get; set; }
        public string AdjustmentType { get; set; }
        public string Remarks { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public virtual Vendors Vendor { get; set; }
        public virtual AdjustmentStatus AdjustmentStatus { get; set; }

    }
}
