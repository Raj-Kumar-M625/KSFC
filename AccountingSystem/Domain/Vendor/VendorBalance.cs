using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Vendor
{
    public class VendorBalance
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Vendor")]
        public int? VendorId { get; set; }

        [DefaultValue("0.00")]
        public decimal OpeningBalance { get; set; }

        [DefaultValue("0.00")]
        public decimal TotalBillAmount { get; set; }

        [DefaultValue("0.00")]
        public decimal TotalNetPayable { get; set; }

        [DefaultValue("0.00")]
        public decimal TotalGST { get; set; }

        [DefaultValue("0.00")]
        public decimal TotalTDS { get; set; }

        [DefaultValue("0.00")]
        public decimal TotalGST_TDS { get; set; }

        [DefaultValue("0.00")]
        public decimal Paid_NetPayable { get; set; }

        [DefaultValue("0.00")]
        public decimal Paid_GST { get; set; }

        [DefaultValue("0.00")]
        public decimal Paid_TDS { get; set; }

        [DefaultValue("0.00")]
        public decimal Paid_GST_TDS { get; set; }

        [DefaultValue("0.00")]
        public decimal Pending_NetPayable { get; set; }

        [DefaultValue("0.00")]
        public decimal Pending_GST { get; set; }

        [DefaultValue("0.00")]
        public decimal Pending_TDS { get; set; }

        [DefaultValue("0.00")]
        public decimal Pending_GST_TDS { get; set; }

        [DefaultValue("0.00")]
        public decimal Pending_BillAmount { get; set; }

        [DefaultValue("0.00")]
        public decimal AmountPaid { get; set; }
        [DefaultValue("0.00")]
        public DateTime? OpeningBalanceDate { get; set; }

        [DefaultValue("0.00")]
        public decimal BalanceAmount { get; set; }

        [DefaultValue("0.00")]
        public decimal Pending_Paid { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public virtual Vendors Vendor { get; set; }
    }
}
