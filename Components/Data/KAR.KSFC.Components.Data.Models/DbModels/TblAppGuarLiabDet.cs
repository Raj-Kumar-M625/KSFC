using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public class TblAppGuarLiabDet
    {
        public int AppGuarLiabId { get; set; }
        public long? PromoterCode { get; set; }
        public long? EgNo { get; set; }
        public int? OffcCd { get; set; }
        public int? UtCd { get; set; }
        public string AppLiabDesc { get; set; } 
        public decimal? AppLiabValue { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
