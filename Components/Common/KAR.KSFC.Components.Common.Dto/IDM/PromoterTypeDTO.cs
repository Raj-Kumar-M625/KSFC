using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM
{
    public class PromoterTypeDTO
    {
        public byte PromTypeId { get; set; }
        public string? PromTypeDets { get; set; }
        public int? PromTypeDisSeq { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        //public virtual IdmSidbiApprovalDTO TblIdmSidbiApproval { get; set; }
    }
}
