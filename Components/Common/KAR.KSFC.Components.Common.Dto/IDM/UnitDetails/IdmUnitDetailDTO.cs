using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.UnitDetails
{
    public class IdmUnitDetailDTO
    {
        public long IdmUtId { get; set; }
        public long? LoanAcc { get; set; }
        //public int? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public int UtCd { get; set; }
        public int UnitDetailsName { get; set; }
        public bool? CnstCd { get; set; }
        public DateTime? IncorporationDt { get; set; }
        public bool? KznCd { get; set; }
        public string? UtZone { get; set; }
        public int? SizeCd { get; set; }
        public string? UnitPan { get; set; }
        public string? UnitGstin { get; set; }
        public int? IndCd { get; set; }
        public string? Name { get; set; }
        public virtual UnitMasterDto TblUnitMast { get; set; }
        //public bool? IsActive { get; set; }
        //public bool? IsDeleted { get; set; }
        //public string CreateBy { get; set; }
        //public string ModifiedBy { get; set; }
        //public DateTime? CreatedDate { get; set; }
        //public DateTime? ModifiedDate { get; set; }
        //public string? UniqueID { get; set; }

    }
}
