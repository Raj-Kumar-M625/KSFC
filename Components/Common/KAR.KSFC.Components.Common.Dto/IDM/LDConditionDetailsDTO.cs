using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM
{
    public class LDConditionDetailsDTO
    {
        public int? SlNo { get; set; }
        public string ConditionStage { get; set; }
        public string ConditionType { get; set; }
        public int? Action { get; set; }
        public long CondDetId { get; set; }
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public byte CondType { get; set; }
        public byte? CondCd { get; set; }
        public long CondStg { get; set; }
        public string CondDetails { get; set; }
        public string Compliance { get; set; }
        public string? CondRemarks { get; set; }
        public bool? WhRelaxation { get; set; }
        public bool? WhRelAllowed { get; set; }
        public string CondUpload { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
        //public virtual ConditionStgDTO TblCondStgCdtab { get; set; }
        public virtual ConditionTypeDTO TblCondTypeCdtab { get; set; }

        public virtual TblCondStageMastDTO TblCondStageMastDTO { get; set; }

    }
}
