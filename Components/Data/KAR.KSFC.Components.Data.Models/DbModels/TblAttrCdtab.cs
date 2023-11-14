using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblAttrCdtab
    {
        public TblAttrCdtab()
        {
            TblDopCdtabs = new HashSet<TblDopCdtab>();
            TblDophistDets = new HashSet<TblDophistDet>();
            TblSubattrCdtabs = new HashSet<TblSubattrCdtab>();
        }

        public int AttrId { get; set; }
        public string AttrDesc { get; set; }
        public int RoleId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual TblRoleCdtab Role { get; set; }
        public virtual ICollection<TblDopCdtab> TblDopCdtabs { get; set; }
        public virtual ICollection<TblDophistDet> TblDophistDets { get; set; }
        public virtual ICollection<TblSubattrCdtab> TblSubattrCdtabs { get; set; }
    }
}
