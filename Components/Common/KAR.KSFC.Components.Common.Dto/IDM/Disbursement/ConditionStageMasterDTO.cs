using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.Disbursement
{
    public class ConditionStageMasterDTO
    {
        public byte CondStgId { get; set; }
        public int? CondStgCode { get; set; }
        public string CondStgDesc { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public virtual AdditionConditionDetailsDTO TblAddlCondDet { get; set; }
    }
}
