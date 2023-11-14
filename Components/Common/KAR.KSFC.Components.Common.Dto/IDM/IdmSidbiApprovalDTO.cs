using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM
{
    public class IdmSidbiApprovalDTO
    {
        public long SidbiApprId { get; set; }
        public byte PromTypeCd { get; set; }
        public string CnstDets { get; set; }
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public decimal? LnSancAmt { get; set; }
        public byte? OffcCd { get; set; }
        public bool WhAppr { get; set; }
        public string? SidbiUpload { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual PromoterTypeDTO TblPromTypeCdtab { get; set; }

    }
}
