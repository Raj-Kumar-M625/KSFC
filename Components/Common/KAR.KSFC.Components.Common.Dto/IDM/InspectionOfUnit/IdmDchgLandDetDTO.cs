using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.InspectionOfUnit
{
    public class IdmDchgLandDetDTO
    {
        public int DcLndRowId { get; set; }
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public int? DcLndArea { get; set; }
        public string DcLndAreaIn { get; set; }
        public int? DcLndType { get; set; }
        public string LandType { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public decimal? DcLndAmt { get; set; }
        public int? DcLndApau { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? DcLndApdt { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public decimal? DcLndDevCst { get; set; }
        public string DcLndLandFinance { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? DcLndStatChgDate { get; set; }
        public int? DcLndAqrdIndicator { get; set; }
        public int? DcLndSecCreated { get; set; }
        public long? DcLndIno { get; set; }
        public long? DcLndrefNo { get; set; }
        public string DcLndDets { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public string IrlAreaIn { get; set; }
        public decimal? IrlSecValue { get; set; }
        public long IrlId { get; set; }
        public int? IrlRelStat { get; set; }
        public string UniqueId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public int? Action { get; set; }
    }
}
