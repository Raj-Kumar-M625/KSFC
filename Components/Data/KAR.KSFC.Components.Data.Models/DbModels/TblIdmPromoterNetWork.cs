using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblIdmPromoterNetWork
    {
        public int PromNetWrId { get; set; }
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public int? UTCD { get; set; }
        public long? PromoterCode { get; set; }
        public decimal? Idminmov { get; set; }
        public decimal? IdmMov { get; set; }
        public decimal? IdmLiab { get; set; }
        public decimal? IdmNetworth { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? UniqueID { get; set; }
    }
}
