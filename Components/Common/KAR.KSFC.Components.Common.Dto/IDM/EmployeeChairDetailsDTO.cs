using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM
{
    public class EmployeeChairDetailsDTO
    {
        public int EmpchairId { get; set; }
        public string EmpId { get; set; }
        public byte OffcCd { get; set; }
        public string TgesCode { get; set; }
        public int ChairCode { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual EmployeeChairDetailsDTO ChairCodeNavigation { get; set; }
    }
}
