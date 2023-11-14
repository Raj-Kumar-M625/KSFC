using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM
{
    public class IdmAuditDetailsDTO
    {
        public int? Action { get; set; }
        public int IdmAuditId { get; set; }
        public long LoanAcc { get; set; }
        public int LoanSub { get; set; }
        public byte OffcCd { get; set; }
        public string AuditObservation { get; set; }
        public string AuditCompliance { get; set; }
        public string AuditUpload { get; set; }
        public int? AuditEmpId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string UniqueId { get; set; }
        public string CreateBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
