using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class ConstmpCdtab
    {
        public ConstmpCdtab()
        {
            VilCdtabs = new HashSet<VilCdtab>();
        }

        public byte ConstmpCd { get; set; }
        public string ConstmpName { get; set; }
        public string ConstmpKannada { get; set; }
        public string ConstmpStateCd { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual ICollection<VilCdtab> VilCdtabs { get; set; }
    }
}
