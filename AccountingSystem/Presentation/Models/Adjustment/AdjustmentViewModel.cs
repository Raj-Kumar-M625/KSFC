using Domain.Adjustment;
using Domain.Vendor;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Presentation.Models.Vendor;

namespace Presentation.Models.Adjustment
{
    public class AdjustmentViewModel
    {
        
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
        public virtual VendorViewModel Vendor { get; set; }
        public virtual AdjustmentStatusModel AdjustmentStatus { get; set; }
    }
}
