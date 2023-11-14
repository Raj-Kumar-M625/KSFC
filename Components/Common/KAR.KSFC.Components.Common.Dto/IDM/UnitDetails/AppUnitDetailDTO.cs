using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.UnitDetails
{
    public class AppUnitDetailDTO
    {
        public int UtRowid { get; set; }
        public int UtCd { get; set; }
        public int? EgNo { get; set; }
        public int? UtName { get; set; }
        public byte? CnstCd { get; set; }
        public DateTime? IncorporationDt { get; set; }
        public byte? KznCd { get; set; }
        public string UtZone { get; set; }
        public int? SizeCd { get; set; }
        public string UnitPan { get; set; }
        public string UnitGstin { get; set; }
        public int IndCd { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual IdmUnitProductsDTO IdmUnitProducts { get; set; }

        public virtual TblIndCdtabDTO ProdIndNavigation { get; set; }



    }
}
