using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblIdmIrBldgMat
    {
        public int IrbmRowId { get; set; }
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public DateTime? IrbmIdt { get; set; }
        public DateTime? IrbmRdt { get; set; }
        public long? IrbmItem { get; set; }
        public string IrbmMat { get; set; }
        public int? IrbmQty { get; set; }
        public decimal? IrbmRate { get; set; }
        public int? IrbmNo { get; set; }
        public string IrbmQtyIn { get; set; }
        public int? UomId { get; set; }
        public long? IrbmIno { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public decimal? IrbmAmt { get; set; }
    }
}
