using KAR.KSFC.Components.Common.Dto.IDM.UnitDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM
{
    public class UnitMasterDto
    {
        public int UtRowId { get; set; }
        public int UtCd { get; set; }
        public string UtName { get; set; }
        public byte UtOffCd { get; set; }
        public string UtUtPan { get; set; }
        public string utFromDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual IdmUnitDetailDTO TblIdmUnitDetails { get; set; }
    }
}
