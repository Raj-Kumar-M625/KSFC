using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class HobCdtab
    {
        public HobCdtab()
        {
            UnitInfo1s = new HashSet<UnitInfo1>();
        }

        public int HobCd { get; set; }
        public string HobNam { get; set; }
        public int? TlqCd { get; set; }
        public string HobNameKannada { get; set; }
        public int? HobLgdcode { get; set; }
        public int? HobBhoomicode { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
        public virtual TlqCdtab TlqCdNavigation { get; set; }
       public virtual ICollection<UnitInfo1> UnitInfo1s { get; set; }
    }
}
