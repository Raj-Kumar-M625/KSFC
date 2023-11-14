using Application.DTOs.GSTTDS;
using Application.DTOs.TDS;
using Application.DTOs.Vendor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable enable
namespace Application.DTOs.Bill
{
    public class BillsDto
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Vendor")]
        public int VendorId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string BillReferenceNo { get; set; }
        public string BillNo { get; set; }
        public decimal TDSWithholdPercent { get; set; }
        public decimal GSTTDSWithholdPercent { get; set; }
        public DateTime BillDate { get; set; }
        public string PaymentTerms { get; set; }
        public DateTime BillDueDate { get; set; }
        public decimal BillAmount { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal GSTAmount { get; set; }
        public decimal? TDS { get; set; }
        public decimal? GSTTDS { get; set; }
        public decimal NetPayable { get; set; }
        public decimal? TotalBillAmount { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int AssementYearCMID { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifedBy { get; set; }
        public string? Remarks { get; set; }

        public decimal BalanceAmount { get; set; }

        public decimal NetPayableAmount { get; set; }
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

        public virtual BillStatusDto BillStatus { get; set; }
        public virtual TdsStatusDto TDSStatus { get; set; }
        public virtual GsttdsStatusDto GSTTDSStatus { get; set; }


        public virtual VendorDetailsDto Vendor { get; set; }

        [NotMapped]
        public string PaymentRefNo { get; set; }

        [NotMapped]
        public decimal OpeningBalancePayableAmount { get; set; }
        public decimal AdvancePaymentUsed { get; set; }
        public decimal PaidAmount { get; set; }
        public string AdvancePayments { get; set; }



    }
}

#nullable disable