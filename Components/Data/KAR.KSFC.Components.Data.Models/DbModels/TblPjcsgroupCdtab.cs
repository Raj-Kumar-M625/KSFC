using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblPjcsgroupCdtab
    {
        public TblPjcsgroupCdtab()
        {
            TblPjcostCdtabs = new HashSet<TblPjcostCdtab>();
        }

        public int PjcsgroupCd { get; set; }
        public string PjcsgroupDets { get; set; }
        public int PjcostgroupCd { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual TblPjcostgroupCdtab PjcostgroupCdNavigation { get; set; }
        public virtual ICollection<TblPjcostCdtab> TblPjcostCdtabs { get; set; }
    }
}
