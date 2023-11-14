using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblChairCdtab
    {
        public TblChairCdtab()
        {
            TblChairmapCdtabFromChaircodeNavigations = new HashSet<TblChairmapCdtab>();
            TblChairmapCdtabToChaircodeNavigations = new HashSet<TblChairmapCdtab>();
            TblEmpchairDets = new HashSet<TblEmpchairDet>();
            TblEmpchairhistDets = new HashSet<TblEmpchairhistDet>();
        }

        public int ChairId { get; set; }
        public int ChairCode { get; set; }
        public string ChairDesc { get; set; }
        public byte OffcCd { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual OffcCdtab OffcCdNavigation { get; set; }
        public virtual ICollection<TblChairmapCdtab> TblChairmapCdtabFromChaircodeNavigations { get; set; }
        public virtual ICollection<TblChairmapCdtab> TblChairmapCdtabToChaircodeNavigations { get; set; }
        public virtual ICollection<TblEmpchairDet> TblEmpchairDets { get; set; }
        public virtual ICollection<TblEmpchairhistDet> TblEmpchairhistDets { get; set; }
    }
}
