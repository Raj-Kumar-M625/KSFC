using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.UnitDetails
{
   public class VilCdTabDTO
    {
        public int VilCd { get; set; }
        public string VilNam { get; set; }
        public int? HobCd { get; set; }
        public string VilNameKannada { get; set; }
        public decimal? VilLgdcode { get; set; }
        public decimal? VilBhoomicode { get; set; }
        public short? ConstmlaCd { get; set; }
        public byte? ConstmpCd { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
        public virtual IdmUnitAddressDTO TblIdmUnitAddress { get; set; } //Added by GK
    }
}
