using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblPjcostgroupCdtab
    {
        public TblPjcostgroupCdtab()
        {
            TblPjcostCdtabs = new HashSet<TblPjcostCdtab>();
            TblPjcsgroupCdtabs = new HashSet<TblPjcsgroupCdtab>();
        }

        public int PjcostgroupCd { get; set; }
        public string PjcostgroupDets { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual ICollection<TblPjcostCdtab> TblPjcostCdtabs { get; set; }
        public virtual ICollection<TblPjcsgroupCdtab> TblPjcsgroupCdtabs { get; set; }
    }
}
