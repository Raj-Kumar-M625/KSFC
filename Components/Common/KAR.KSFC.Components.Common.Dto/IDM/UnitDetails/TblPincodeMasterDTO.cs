using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.UnitDetails
{
    public class TblPincodeMasterDTO
    {
        public int PincodeRowId { get; set; }
        public int PincodeCd { get; set; }
        public string? PincodeState { get; set; }
        public string? PincodeDistrict { get; set; }
        public int PincodeDistrictCd { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
