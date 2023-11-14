using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.UnitDetails
{
    public class HobCdtabDTO
    {
        public int HobCd { get; set; }
        public string HobNam { get; set; }
        public int? TlqCd { get; set; }
        public string HobNameKannada { get; set; }
        public int? HobLgdcode { get; set; }
        public int? HobBhoomicode { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
        //public virtual TlqCdTabDTO TlqCdtab { get; set; }
        public virtual IdmUnitAddressDTO TblIdmUnitAddress { get; set; }
    }
}
