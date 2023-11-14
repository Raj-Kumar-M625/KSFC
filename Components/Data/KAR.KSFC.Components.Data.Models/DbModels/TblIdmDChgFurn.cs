using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblIdmDChgFurn
    {
        public long Id { get; set; }
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public string? FurnDetails { get; set; }
        public string? FurnSupp { get; set; }
        public string? FurnSuppAdd1 { get; set; }
        public string? FurnSuppAdd2 { get; set; }
        public string? FurnSuppAdd3 { get; set; }
        public int? FurnInvoiceNo { get; set; }
        public DateTime? FurnInvoiceDate { get; set; }
        public int? FurnCletStat { get; set; }
        public int? FurnReg { get; set; }
        public int? FurnQty { get; set; }
        public int? Stat { get; set; }
        public int? FurnDeleiverInWeek { get; set; }
        public int? FurnCost { get; set; }
        public decimal? FurnTax { get; set; }
        public decimal? FurnTotalCost { get; set; }
        public int? FurnItemNo { get; set; }
        public int? FurnAqrdStat { get; set; }
        public DateTime? FurnRequDate { get; set; }
        public int? FurnApdt { get; set; }
        public int? FurnApau { get; set; }
        public int? FurnNonSsi { get; set; }
        public decimal? FurnActualCost { get; set; }
        public int? FurnAqrdIndicator { get; set; }
        public DateTime? FurnStatChangeDate { get; set; }
        public string? FurnBankAdvice { get; set; }
        public string? FurnBankAdvDate { get; set; }
        public string? FurnBankName { get; set; }
        public  int? FurnSat { get; set; }
        public  int? FurnSec { get; set; }
        public  long? FurnIno { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? UniqueID { get; set; }
        public decimal? DfurnSecRel { get; set; }
    }
}
