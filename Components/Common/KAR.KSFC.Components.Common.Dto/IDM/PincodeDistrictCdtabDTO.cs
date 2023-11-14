using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM
{
   public class PincodeDistrictCdtabDTO
    {
        public int PincodeDistrictCd { get; set; }
        public string? PincodeDistrictDesc { get; set; }
        public int PincodeStateCd { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public byte? Distcd { get; set; }

        public virtual PincodeStateCdtabDTO PincodeStateCdtab { get; set; } 
    }
}
