using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.CreationOfSecurityandAquisitionAsset
{
    public class TblIdmBuildingAcquisitionDetailsDTO
    {
        public int Irbid { get; set; }
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public DateTime? IrbIdt { get; set; }
        public long? IrbItem { get; set; }
        public int? IrbArea { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public decimal? IrbValue { get; set; }
        public int? IrbNo { get; set; }
        public long? IrbIno { get; set; }
        public int? IrbStatus { get; set; }
        public int? IrbSecValue { get; set; }
        public int? IrbRelStat { get; set; }
        public int? IrbAPArea { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public decimal? IrbATCost { get; set; }
        public int? IrbPercentage { get; set; }
        public string IrbBldgConstStatus { get; set; }
        public string IrbBldgDetails { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public decimal? IrbUnitCost { get; set; }
        public string RoofType { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public decimal? IrbCost { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }    
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
        public int Action { get; set; }
    }
}
