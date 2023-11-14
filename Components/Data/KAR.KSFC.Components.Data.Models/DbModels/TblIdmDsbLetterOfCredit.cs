using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public class TblIdmDsbLetterOfCredit
    {
        public long DcLocRowId { get; set; }
        public long LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public int? DlCrdtItmNo { get; set; }
        public string DlCrdtItmDets { get; set; }
        public string DlCrdtCrltNo { get; set; }
        public DateTime? DlCrdtDt { get; set; }
        public decimal? DlCrdtAmt { get; set; }
        public decimal? DlCrdtCif { get; set; }
        public string DlCrdtBank { get; set; }
        public string DlCrdtBnkadr1 { get; set; }
        public string DlCrdtBnkadr2 { get; set; }
        public string DlCrdtBnkadr3 { get; set; }
        public DateTime? DlCrdtRqdt { get; set; }
        public DateTime? DlCrdtOpenDt { get; set; }
        public DateTime? DlCrdtVdty { get; set; }
        public DateTime? DlCrdtHondt { get; set; }
        public int? DlCrdtAu { get; set; }
        public int? DlCrdtCletStat { get; set; }
        public decimal? DlCrdtMrgMny { get; set; }
        public string DlCrdtSup { get; set; }
        public string DlCrdtSupAddr { get; set; }
        public int? DlCrdtAqrdStat { get; set; }
        public decimal? DlCrdtTotalAmt { get; set; }
        public string DlCrdtBankIfsc { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
