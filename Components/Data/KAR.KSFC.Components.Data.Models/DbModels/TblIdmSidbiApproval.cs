using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public class TblIdmSidbiApproval
    {
        public long SidbiApprId { get; set; }
        public byte PromTypeCd { get; set; }
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public decimal? LnSancAmt { get; set; }
        public byte? OffcCd { get; set; }
        public bool WhAppr { get; set; }
        public string? SidbiUpload { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        //public virtual TblAppLoanMast TblAppLoanMast { get; set; }
        //public virtual OffcCdtab OffcCdtab { get; set; }
        public virtual TblPromTypeCdtab TblPromTypeCdtab { get; set; }

    }
}
