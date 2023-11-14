using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblSizeCdtab
    {
        public TblSizeCdtab()
        {
            TblEnqBasicDets = new HashSet<TblEnqBasicDet>();
            UnitInfo1s = new HashSet<UnitInfo1>();
        }

        public int SizeCd { get; set; }
        public string SizeDets { get; set; }
        public int? SizeFlag { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual ICollection<TblEnqBasicDet> TblEnqBasicDets { get; set; }
        public virtual ICollection<UnitInfo1> UnitInfo1s { get; set; }
    }
}
