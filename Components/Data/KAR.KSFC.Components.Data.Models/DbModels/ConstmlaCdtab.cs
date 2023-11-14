using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class ConstmlaCdtab
    {
        public ConstmlaCdtab()
        {
            VilCdtabs = new HashSet<VilCdtab>();
        }

        public short ConstmlaCd { get; set; }
        public string ConstmlaName { get; set; }
        public string ConstmlaKannada { get; set; }
        public string ConstmlaStateCd { get; set; }
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
