using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.UnitDetails
{
    public class DistCdTabDTO
    {
        public DistCdTabDTO()
        {
            TlqCdtabs = new HashSet<TlqCdTabDTO>();
        }

        public byte DistCd { get; set; }
        public string DistNam { get; set; }
        public string DistZone { get; set; }
        public string DistFbFlg { get; set; }
        public byte? DistOffcCd { get; set; }
        public byte? DistZoneCd { get; set; }
        public byte? DistCircle { get; set; }
        public string DistNameKannada { get; set; }
        public int? DistLgdcode { get; set; }
        public int? DistBhoomicode { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
        public virtual ICollection<TlqCdTabDTO> TlqCdtabs { get; set; }
        public virtual IdmUnitAddressDTO TblIdmUnitAddress { get; set; } 
    }
}
