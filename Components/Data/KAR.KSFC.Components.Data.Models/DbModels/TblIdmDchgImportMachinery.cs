using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public class TblIdmDchgImportMachinery
    {
        public long DimcRowId { get; set; }
        public long LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public long? DimcItmNo { get; set; }
        public string DimcEntry { get; set; }
        public string DimcEntryI { get; set; }
        public string DimcCrncy { get; set; }
        public long? DimcExrd { get; set; }
        public long? DimcCif { get; set; }
        public long? DimcCduty { get; set; }
        public decimal? DimcTotCost { get; set; }
        public int? DimcStat { get; set; }
        public string DimcDets { get; set; }
        public string DimcSup { get; set; }
        public long? DimcQty { get; set; }
        public long? DimcDelry { get; set; }
        public string DimcSupAdr1 { get; set; }
        public string DimcSupAdr2 { get; set; }
        public string DimcSupAdr3 { get; set; }
        public long? DimcCpct { get; set; }
        public decimal? DimcCamt { get; set; }
        public int? DimcAqrdStat { get; set; }
        public long? DimcApau { get; set; }
        public DateTime? DimcApDate { get; set; }
        public string DimcCletStat { get; set; }
        public decimal? DimcActualCost { get; set; }
        public long? DimcAqrdIndicator { get; set; }
        public string DimcBankAdivce { get; set; }
        public DateTime? DimcBankAdvDate { get; set; }
        public string DimcMacDocuments { get; set; }
        public DateTime? DimcStatChgDate { get; set; }
        public long? Dimcsec { get; set; }
        public long? DimcIno { get; set; }
        public long? DimcsecRel { get; set; }
        public long? DimcsecElig { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string DimcStatDesc { get; set; }
        public decimal? DimcGST { get; set; }
        public decimal? DimcOthExp { get; set; }
        public decimal? DimcTotalCostMem { get; set; }
        public decimal? DimcCurBasicAmt { get; set; }
        public decimal? DimcBasicAmt { get; set; }
    }
}
