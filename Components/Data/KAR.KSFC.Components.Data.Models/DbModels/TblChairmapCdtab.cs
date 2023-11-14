using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblChairmapCdtab
    {
        public int ChairmapId { get; set; }
        public int FromChaircode { get; set; }
        public int ToChaircode { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual TblChairCdtab FromChaircodeNavigation { get; set; }
        public virtual TblChairCdtab ToChaircodeNavigation { get; set; }
    }
}
