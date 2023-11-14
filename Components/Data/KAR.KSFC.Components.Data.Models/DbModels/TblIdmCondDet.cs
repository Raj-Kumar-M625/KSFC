using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblIdmCondDet
    {
        public long CondDetId { get; set; }
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public byte CondType { get; set; }
        public byte? CondCd { get; set; }
        public long CondStg { get; set; }
        public string CondDetails { get; set; }
        public string Compliance { get; set; }
        public string CondRemarks { get; set; }
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
        //public virtual TblCondStgCdtab TblCondStgCdtab { get; set; }
        public virtual TblCondTypeCdtab TblCondTypeCdtab { get; set; }

        public virtual TblCondStageMast TblCondStageMast { get; set; }

    }
}
