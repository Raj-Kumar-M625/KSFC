using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblRoleCdtab
    {
        public TblRoleCdtab()
        {
            TblAttrCdtabs = new HashSet<TblAttrCdtab>();
            TblDopCdtabs = new HashSet<TblDopCdtab>();
            TblDophistDets = new HashSet<TblDophistDet>();
        }

        public int RoleId { get; set; }
        public string RoleDesc { get; set; }
        public int ModuleId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual TblModuleCdtab Module { get; set; }
        public virtual ICollection<TblAttrCdtab> TblAttrCdtabs { get; set; }
        public virtual ICollection<TblDopCdtab> TblDopCdtabs { get; set; }
        public virtual ICollection<TblDophistDet> TblDophistDets { get; set; }
    }
}
