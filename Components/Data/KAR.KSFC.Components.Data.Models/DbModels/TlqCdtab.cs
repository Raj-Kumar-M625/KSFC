using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TlqCdtab
    {
        public TlqCdtab()
        {
            HobCdtabs = new HashSet<HobCdtab>();
        }

        public int TlqCd { get; set; }
        public string TlqNam { get; set; }
        public byte? DistCd { get; set; }
        public byte? TlqIndzone { get; set; }
        public string TlqNameKannada { get; set; }
        public int? TlqLgdcode { get; set; }
        public int? TlqBhoomicode { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
        public virtual DistCdtab DistCdNavigation { get; set; }
       public virtual ICollection<HobCdtab> HobCdtabs { get; set; }
     
    }
}
