using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.InspectionOfUnit
{
    public class IdmDsbLetterOfCreditDTO
    {
        public long DcLocRowId { get; set; }
        public long LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public int? DlCrdtItmNo { get; set; }
        public string DlCrdtItmDets { get; set; }
        public string DlCrdtCrltNo { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? DlCrdtDt { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public decimal? DlCrdtAmt { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public decimal? DlCrdtCif { get; set; }
        public string DlCrdtBank { get; set; }
        public string DlCrdtBnkadr1 { get; set; }
        public string DlCrdtBnkadr2 { get; set; }
        public string DlCrdtBnkadr3 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? DlCrdtRqdt { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? DlCrdtOpenDt { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? DlCrdtVdty { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? DlCrdtHondt { get; set; }
        public int? DlCrdtAu { get; set; }
        public int? DlCrdtCletStat { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public decimal? DlCrdtMrgMny { get; set; }
        public string DlCrdtSup { get; set; }
        public string DlCrdtSupAddr { get; set; }
        public int? DlCrdtAqrdStat { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public decimal? DlCrdtTotalAmt { get; set; }
        public string DlCrdtBankIfsc { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public int? Action { get; set; }
    }
}
