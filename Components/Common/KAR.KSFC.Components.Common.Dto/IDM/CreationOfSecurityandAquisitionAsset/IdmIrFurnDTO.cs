using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.CreationOfSecurityandAquisitionAsset
{
    public class TblIdmIrFurnDTO
    {
        public long IrfId { get; set; }
        public long LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public DateTime? IrfIDT { get; set; }
        public DateTime? IrfRDT { get; set; }
        public long? IrfItem { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public decimal? IrfAmt { get; set; }
        public int? IrfNo { get; set; }
        public bool? IrfAqrdStat { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public int? IrfSecAmt { get; set; }
        public int? IrfRelStat { get; set; }
        public int? IrfAamt { get; set; }
        public int? IrfTotalRelease { get; set; }
        public string IrfItemDets { get; set; }
        public string IrfSupplier { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
        public long? IrfIno { get; set; }
        public int? Action { get; set; }
        //public virtual LoanAccountNumberDTO LoanAccount { get; set; }
        //public virtual OfficeDto OffCd { get; set; }
    }
}
