using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.Bill
{
    public class BillItemsModel
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
        public decimal? NetPayable { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifedBy { get; set; }
        [ForeignKey("Bill")]
        public int BillsID { get; set; }
        public List<string>? DocumentName { get; set; }
        public List<IFormFile> Attachment { get; set; }
        public List<IFormFile> File { get; set; }
        public virtual BillModel Bills { get; set; }
        public virtual BillModel BillDate { get; set; }
        public virtual BillModel BillDueDate { get; set; }
        public virtual BillModel BillNo { get; set; }


    }
}
