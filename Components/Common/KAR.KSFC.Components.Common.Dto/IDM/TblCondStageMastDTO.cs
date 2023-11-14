using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM
{
    public class TblCondStageMastDTO
    {
        public long CondStageId { get; set; }
        public int? CondStg { get; set; }
        public string CondStgDets { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string? CreateBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual LDConditionDetailsDTO LDConditionDetailsDTO { get; set; }
    }
}
