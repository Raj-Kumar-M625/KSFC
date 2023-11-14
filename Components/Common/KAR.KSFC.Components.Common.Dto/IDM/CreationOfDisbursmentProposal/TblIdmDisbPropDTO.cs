using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.CreationOfDisbursmentProposal
{
    public class TblIdmDisbPropDTO
    {
        public long PropId { get; set; }
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public long? PropNumber { get; set; }
        public DateTime? PropDate { get; set; }
        public int? PropDept { get; set; }
        public int? PropLoanType { get; set; }
        public int? PropSancAmount { get; set; }
        public int? PropDisbAmount { get; set; }
        public int? PropRecAmount { get; set; }

        public int? PropStatusFlg { get; set; }
        public string PropFdsbFlg { get; set; }
        public string PropReltyFlg { get; set; }

        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual TblIdmReleDetlsDTO TblIdmReleDetls { get; set; }

    }
}
