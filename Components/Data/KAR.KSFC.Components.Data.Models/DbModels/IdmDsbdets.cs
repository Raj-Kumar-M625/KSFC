using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class IdmDsbdets
    {
        public long DsbdetsID { get; set; }
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public int? OffcCd { get; set; }
        public int? DsbNo { get; set; }
        public DateTime? DsbDt { get; set; }
        public decimal? DsbAmt { get; set; }
        public int? DsbAcd { get; set; }
        public decimal? DsbEstAmt { get; set; }
        public decimal? SecConsideredFRelease { get; set; }
        public decimal? SecInspection { get; set; }
        public int? MarginRetained { get; set; }
        public decimal? AlocAmt { get; set; }

        public decimal? PropAmt { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

    }
}
