using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    /// <summary>
    ///  Author: Gagana K; Module: ScurityCharge; Date:21/07/2022
    /// </summary>
    public partial class TblSecurityRefnoMast
    {
        public int SecRefnoMastId { get; set; }
        public int SecurityCd { get; set; }
        public string SecurityDetails { get; set; }
        public decimal? SecurityValue { get; set; }
        public string SecNameHolder { get; set; }
        public string UniqueId { get; set; }
        public short? SecCd { get; set; }
        public int? PjsecCd { get; set; }
        public byte? OffcCd { get; set; }
        public int? LoanSub { get; set; }
        public long LoanAcc { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public bool? WhCharge { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public virtual TblIdmDeedDet TblIdmDeedDet { get; set; }
        public virtual TblIdmDsbCharge TblIdmDsbCharge { get; set; }
        public virtual TblSecCdtab TblSecCdtab { get; set; }
        public virtual TblPjsecCdtab TblPjsecCdtab { get; set; }
    }
}
