using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblEnqPjcostDet
    {
        public int EnqPjcostId { get; set; }
        public int EnqtempId { get; set; }
        public int PjcostCd { get; set; }
        public decimal? EnqPjcostAmt { get; set; }
        public string EnqPjcostRem { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual TblEnqTemptab Enqtemp { get; set; }
        public virtual TblPjcostCdtab PjcostCdNavigation { get; set; }
    }
}
