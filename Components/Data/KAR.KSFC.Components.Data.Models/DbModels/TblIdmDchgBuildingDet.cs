using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public class TblIdmDchgBuildingDet
    {
        public long DcBdgRowId { get; set; }
        public long LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public long? DcBdgItmNo { get; set; }
        public string? DcBdgDets { get; set; }
        public string? DcBdgRoof { get; set; }
        public decimal? DcBdgUcost { get; set; }
        public decimal? DcBdgTcost { get; set; }
        public int? DcBdgPlnth { get; set; }
        public DateTime? DcBdgRqdt { get; set; }
        public DateTime? DcBdgApdt { get; set; }
        public int? DcBdgApau { get; set; }
        public DateTime? DcBdgComdt { get; set; }
        public int? DcBdgRqrdStat { get; set; }
        public int? DcBdgStat { get; set; }
        public DateTime? DcBdgStatChgDate { get; set; }
        public int? DcBdgSecCreatd { get; set; }
        public int? DcBdgAplnth { get; set; }
        public int? DcBdgAtCost { get; set; }
        public int? DcBdgPercent { get; set; }
        public long DcBdgIno { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }
}

