using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class VilCdtab
    {
        public VilCdtab()
        {
            TblEnqBasicDets = new HashSet<TblEnqBasicDet>();
            UnitInfo1s = new HashSet<UnitInfo1>();
        }

        public int VilCd { get; set; }
        public string VilNam { get; set; }
        public int? HobCd { get; set; }
        public string VilNameKannada { get; set; }
        public decimal? VilLgdcode { get; set; }
        public decimal? VilBhoomicode { get; set; }
        public short? ConstmlaCd { get; set; }
        public byte? ConstmpCd { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual ConstmlaCdtab ConstmlaCdNavigation { get; set; }
        public virtual ConstmpCdtab ConstmpCdNavigation { get; set; }
        public virtual ICollection<TblEnqBasicDet> TblEnqBasicDets { get; set; }
        public virtual ICollection<UnitInfo1> UnitInfo1s { get; set; }
    }
}
