using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.Disbursement
{
    public class AdditionConditionDetailsDTO
    {
        public long AddCondId { get; set; }
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public byte? AddCondCode { get; set; }
        public byte? AddCondStage { get; set; }
        public string AddCondDetails { get; set; }
        public bool? Relaxation { get; set; }
        public bool? WhRelAllowed { get; set; }
        public string Compliance { get; set; }
        public string UniqueId { get; set; }
        public string ConditionStage { get; set; }
        public int? Action { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ConditionStgDTO TblCondStgCdtab { get; set; }


    }
  
}
