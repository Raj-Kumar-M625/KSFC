using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Vendor
{
  public class VendorBalanceDto
    {

        [Key]
        public int Id { get; set; }

        [ForeignKey("Vendor")]
        public int VendorId { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal TotalBillAmount { get; set; }
        public decimal TotalNetPayable { get; set; }
        public decimal TotalGST { get; set; }
        public decimal TotalTDS { get; set; }
        public decimal TotalGST_TDS { get; set; }
        public decimal Paid_NetPayable { get; set; }
        public decimal Paid_GST { get; set; }
        public decimal Paid_TDS { get; set; }
        public decimal Paid_GST_TDS { get; set; }
        public decimal Pending_NetPayable { get; set; }
        public decimal Pending_GST { get; set; }
        public decimal Pending_TDS { get; set; }
        public decimal Pending_GST_TDS { get; set; }
        public decimal Pending_BillAmount { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal BalanceAmount { get; set; }
        public decimal Pending_Paid { get; set; }
        public DateTime OpeningBalanceDate { get; set; }
        
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public virtual VendorDetailsDto Vendor { get; set; }
    }
}
