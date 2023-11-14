using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.UnitDetails
{
    public class TblIndCdtabDTO
    {
        public int IndCd { get; set; }
        public string? IndDets { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? UniqueId { get; set; }

        public virtual AppUnitDetailDTO UtCdNavigation { get; set; }

        public virtual TblProdCdtabDTO TblProdCdtabs { get; set; }

        public virtual IdmUnitProductsDTO IdmUnitProducts { get; set; }

    }
}
