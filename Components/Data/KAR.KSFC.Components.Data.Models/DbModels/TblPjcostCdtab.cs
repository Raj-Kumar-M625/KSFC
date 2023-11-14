using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblPjcostCdtab
    {
        public TblPjcostCdtab()
        {
            TblEnqPjcostDets = new HashSet<TblEnqPjcostDet>();
        }

        public int PjcostCd { get; set; }
        public string PjcostDets { get; set; }
        public int? PjcostFlg { get; set; }
        public decimal? SeqNo { get; set; }
        public int? PjcostgroupCd { get; set; }
        public int? PjcsgroupCd { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual TblPjcostgroupCdtab PjcostgroupCdNavigation { get; set; }
        public virtual TblPjcsgroupCdtab PjcsgroupCdNavigation { get; set; }
        public virtual ICollection<TblEnqPjcostDet> TblEnqPjcostDets { get; set; }
    }
}
