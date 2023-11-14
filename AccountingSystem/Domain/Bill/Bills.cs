using Domain.GSTTDS;
using Domain.TDS;
using Domain.Vendor;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Bill
{
    public class Bills
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Vendor")]
        public int VendorId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string BillReferenceNo { get; set; }
        public string BillNo { get; set; }

        public decimal? TDSWithholdPercent { get; set; }
        public decimal? GSTTDSWithholdPercent { get; set; }
        public DateTime BillDate { get; set; }
        public string? PaymentTerms { get; set; }
        public DateTime BillDueDate { get; set; }
        public decimal BillAmount { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal GSTAmount { get; set; }
        public decimal TDS { get; set; }
        public decimal GSTTDS { get; set; }
        public decimal NetPayable { get; set; }
        public decimal TotalBillAmount { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int AssementYearCMID { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifedBy { get; set; }

        public string? Remarks { get; set; }

        public decimal BalanceAmount { get; set; }
        public decimal? NetPayableAmount { get; set; }
        public decimal Royalty { get; set; }
        public decimal CBF { get; set; }
        public decimal LabourWelfareCess { get; set; }
        public decimal? Penalty { get; set; }
        public string Other1 { get; set; }
        public decimal? Other1Value { get; set; }
        public string Other2 { get; set; }
        public decimal? Other2Value { get; set; }
        public string Other3 { get; set; }
        public decimal? Other3Value { get; set; }

        public virtual BillStatus BillStatus { get; set; }

        public virtual TdsStatus TDSStatus { get; set; }

        public virtual GsttdsStatus GSTTDSStatus { get; set; }

        //public virtual CommonMaster AssementYearMaster {get;set;}
        public virtual Vendors Vendor { get; set; }

        [NotMapped]
        public string PaymentRefNo { get; set; }

    }
}
