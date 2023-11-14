using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public class IdmUnitProducts
    {
        public long IdmUtproductRowid { get; set; }
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
      
        public int ProdCd { get; set; }

        public string UniqueId { get; set; }
        public int ProdId { get; set; }

        public int IndId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
       

        public virtual TblProdCdtab TblProdCdtabs { get; set; }

        public virtual TblIndCdtab ProdIndNavigation { get; set; }

    }
}
