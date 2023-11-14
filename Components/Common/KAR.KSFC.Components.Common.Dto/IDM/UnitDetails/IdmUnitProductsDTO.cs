using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.UnitDetails
{
    public class IdmUnitProductsDTO
    {
        public long IdmUtproductRowid { get; set; }
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public int UtCd { get; set; }

        public string? IndDets { get; set; }
        public int Id { get; set; }
        public string ProdDets { get; set; }
        public int ProdCd { get; set; }
        public int ProdId { get; set; }

        public int IndId { get; set; }
        public string UniqueId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? Action { get; set; }
        public virtual AppUnitDetailDTO UtCdNavigation { get; set; }
        public virtual TblProdCdtabDTO TblProdCdtabs { get; set; }
        public virtual TblIndCdtabDTO ProdIndNavigation { get; set; }

    }
}
