using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblAppLoanMast
    {
        public long InMastId { get; set; }
        public byte? InOffc { get; set; }
        public int? InUnit { get; set; }
        public int? InSno { get;set; }
        public long? InNo { get; set; }
        public int? InTy { get; set; }
        public decimal? InSanAmt { get; set; }
        public DateTime? InSanDt { get; set; }
        public int? InSchm { get; set; }
        public int? InStat { get; set; }
        public decimal? InIntrLow { get; set; }
        public decimal? InIntrHigh { get; set; }
        public decimal? InIntReb { get; set; }
        public int? InPmode { get; set; }
        public int? InImode { get; set; }
        public int? InMortPrd { get; set; }
        public int? UnitId { get; set; }
        public int? InPurTy { get; set; }
        public int? InSub { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
        public virtual OffcCdtab OffcCdtab { get; set; }
        public virtual TblUnitMast TblUnitMast { get; set; }
        public virtual TblIdmLegalWorkflow TblIdmLegalWorkflow { get; set; } //by gowtham s on 2/8/22

        // public virtual TblEmpchairDet TblEmpchairDets { get; set; } //by gowtham s on 2/8/22

        //public virtual TblIdmSidbiApproval TblIdmSidbiApproval { get; set; } // By Dev on 19/08/2022
       
    }
}
