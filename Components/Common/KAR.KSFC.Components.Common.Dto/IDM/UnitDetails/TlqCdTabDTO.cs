using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.UnitDetails
{
    public class TlqCdTabDTO
    {
        public TlqCdTabDTO()
        {
            HobCdtabs = new HashSet<HobCdtabDTO>();
        }
        public int TlqCd { get; set; }
        public string TlqNam { get; set; }
        public byte? DistCd { get; set; }
        public byte? TlqIndzone { get; set; }
        public string TlqNameKannada { get; set; }
        public int? TlqLgdcode { get; set; }
        public int? TlqBhoomicode { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
        public virtual DistCdTabDTO DistCdtab { get; set; }
       public virtual ICollection<HobCdtabDTO> HobCdtabs { get; set; }
       
    }
}
