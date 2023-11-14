using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.InspectionOfUnit
{
    public class IdmDchgBuildingDetDTO
    {
        public long DcBdgRowId { get; set; }
        public long LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public int? Action { get; set; }
        public byte? OffcCd { get; set; }
        public long? DcBdgItmNo { get; set; }
        public string? DcBdgDets { get; set; }
        public string? DcBdgRoof { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public decimal? DcBdgTcost { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public decimal? DcBdgUcost { get; set; }
        public long? DcBdgPlnth { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? DcBdgRqdt { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? DcBdgApdt { get; set; }
        public int? DcBdgApau { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? DcBdgComdt { get; set; }
        public int? DcBdgRqrdStat { get; set; }
        public int? DcBdgStat { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? DcBdgStatChgDate { get; set; }
        public int? DcBdgSecCreatd { get; set; }
        public long? DcBdgAplnth { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public decimal? DcBdgAtCost { get; set; }
        public int? DcBdgPercent { get; set; }
        public long DcBdgIno { get; set; }
        public decimal? IrbUnitCost { get; set; }
        public int? IrbPercentage { get; set; }

        public int? IrbSecValue { get; set; }
        public string IrbBldgConstStatus { get; set; }
        public string RoofType { get; set; }
        public int Irbid { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
