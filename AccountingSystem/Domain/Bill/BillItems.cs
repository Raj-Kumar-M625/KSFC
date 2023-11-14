using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Bill
{
    public class BillItems
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Vendor")]
        public int VendorID { get; set; }
        public string BillReferenceNo { get; set; }
        public string Category { get; set; }
        public decimal GSTSWithholdPercent { get; set; }
        public decimal Amount { get; set; }
        public decimal GSTAmount { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public string Description { get; set; }
        public decimal TDS { get; set; }
        public decimal GSTTDS { get; set; }
        public decimal TotalNetPayable { get; set; }
        public decimal NetPayable { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifedBy { get; set; }
        [ForeignKey("Bill")]
        public int BillsID { get; set; }
        public virtual Bills Bills { get; set; }

        [NotMapped]
        public string PaymentRefNo { get; set; }
    }
}
