using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblSubattrCdtab
    {
        public TblSubattrCdtab()
        {
            TblDopCdtabs = new HashSet<TblDopCdtab>();
            TblDophistDets = new HashSet<TblDophistDet>();
        }

        public int SubattrId { get; set; }
        public string SubattrDesc { get; set; }
        public int AttrId { get; set; }
        public int AttrunitId { get; set; }
        public int UnitoptrId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual TblAttrCdtab Attr { get; set; }
        public virtual TblAttrunitCdtab Attrunit { get; set; }
        public virtual TblUnitoptrCdtab Unitoptr { get; set; }
        public virtual ICollection<TblDopCdtab> TblDopCdtabs { get; set; }
        public virtual ICollection<TblDophistDet> TblDophistDets { get; set; }
    }
}
