using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class DistCdtab
    {
        public DistCdtab()
        {
            OffcCdtabs = new HashSet<OffcCdtab>();
            TlqCdtabs = new HashSet<TlqCdtab>();
        }

        public byte DistCd { get; set; }
        public string DistNam { get; set; }
        public string DistZone { get; set; }
        public string DistFbFlg { get; set; }
        public byte? DistOffcCd { get; set; }
        public byte? DistZoneCd { get; set; }
        public byte? DistCircle { get; set; }
        public string DistNameKannada { get; set; }
        public int? DistLgdcode { get; set; }
        public int? DistBhoomicode { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
        public virtual KznCdtab DistCircleNavigation { get; set; }
        public virtual ICollection<OffcCdtab> OffcCdtabs { get; set; }
        public virtual ICollection<TlqCdtab> TlqCdtabs { get; set; }
    }
}
