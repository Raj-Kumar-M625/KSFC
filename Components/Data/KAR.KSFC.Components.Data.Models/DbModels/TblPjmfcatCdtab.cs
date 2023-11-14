using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblPjmfcatCdtab
    {
        public TblPjmfcatCdtab()
        {
            TblEnqPjmfDets = new HashSet<TblEnqPjmfDet>();
            TblPjmfCdtabs = new HashSet<TblPjmfCdtab>();
        }

        public int MfcatCd { get; set; }
        public string PjmfDets { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual ICollection<TblEnqPjmfDet> TblEnqPjmfDets { get; set; }
        public virtual ICollection<TblPjmfCdtab> TblPjmfCdtabs { get; set; }
    }
}
