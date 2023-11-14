using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblIdmDchgIndigenousMachineryDet
    {
        public long Id { get; set; }
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime? ApproveDate { get; set; }
        public int? ApproveAU { get; set; }
        public int? ItemNo { get; set; }
        public string ItemDetails { get; set; }
        public string SupplierName { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int? RegisteredState { get; set; }
        public int? Quantity { get; set; }
        public string Status { get; set; }
        public int? Delivery { get; set; }
        public decimal? Cost { get; set; }
        public int? Tax { get; set; }
        public decimal? TotalCost { get; set; }
        public DateTime? CommunicationDate { get; set; }
        public int? RequestNo { get; set; }
        public string SupplierAddress1 { get; set; }
        public string SupplierAddress2 { get; set; }
        public string SupplierAddress3 { get; set; }
        public int? MachineryStatus { get; set; }
        public int? AquiredStatus { get; set; }
        public int? ActualCost { get; set; }
        public string BankAdvice { get; set; }
        public string BankName { get; set; }
        public DateTime? BankAdviceDate { get; set; }
        public int? AquiredIndicator { get; set; }
        public DateTime? StatusChangedDate { get; set; }
        public string CletValidity { get; set; }
        public int? SecurityCreated { get; set; }
        public int? Ino { get; set; }
        public string SecurityRelease { get; set; }
        public bool? SecurityEligibility { get; set; }
        public decimal? BasicCost { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
