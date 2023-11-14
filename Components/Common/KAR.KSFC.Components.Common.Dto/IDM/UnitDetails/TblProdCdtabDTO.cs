using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.UnitDetails
{
    public class TblProdCdtabDTO
    {
        public int ProdInd { get; set; }
        public int ProdCd { get; set; }
        public string ProdDets { get; set; }
        public string Dept { get; set; }
        public int? ProdNcd { get; set; }
        public string ProdNdt { get; set; }
        public int? ProfFlg { get; set; }
        public int Id { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string UniqueId { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        public virtual IdmUnitProductsDTO IdmUnitProducts { get; set; }

        public virtual TblIndCdtabDTO ProdIndNavigation { get; set; }
    }
}
