using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class KznCdtab
    {
        public KznCdtab()
        {
            DistCdtabs = new HashSet<DistCdtab>();
            OffcCdtabs = new HashSet<OffcCdtab>();
        }

        public byte KznCd { get; set; }
        public string KznNam { get; set; }
        public string KznAdr1 { get; set; }
        public string KznAdr2 { get; set; }
        public string KznAdr3 { get; set; }
        public int? KznPin { get; set; }
        public long? KznTel { get; set; }
        public string KznTlx { get; set; }
        public string KznFax { get; set; }
        public bool? KznFlag { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual ICollection<DistCdtab> DistCdtabs { get; set; }
        public virtual ICollection<OffcCdtab> OffcCdtabs { get; set; }
    }
}
