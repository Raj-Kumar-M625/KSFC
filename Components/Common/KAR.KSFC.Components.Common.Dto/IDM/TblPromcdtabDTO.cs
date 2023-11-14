using KAR.KSFC.Components.Common.Dto.IDM.UnitDetails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM
{
    public class TblPromcdtabDTO
    {
        public long PromoterCode { get; set; }
        public string PromoterPan { get; set; }
        public string PromoterName { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? PromoterDob { get; set; }
        public string PromoterGender { get; set; }
        public string PromoterPassport { get; set; }
        public string PromoterPhoto { get; set; }
        public DateTime? PanValidationDate { get; set; }
        public string PromoterEmailid { get; set; }
        public long? PromoterMobno { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual IdmPromAssetDetDTO IdmPromAssetDet { get; set; }
        public virtual IdmPromoterDTO IdmPromoter { get; set; } 
    }
}
