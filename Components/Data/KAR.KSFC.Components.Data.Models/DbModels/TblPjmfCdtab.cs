using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblPjmfCdtab
    {
        public TblPjmfCdtab()
        {
            TblEnqPjmfDets = new HashSet<TblEnqPjmfDet>();
        }

        public int PjmfCd { get; set; }
        public string PjmfDets { get; set; }
        public byte? PjmfFlg { get; set; }
        public int? MfcatCd { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual TblPjmfcatCdtab MfcatCdNavigation { get; set; }
        public virtual ICollection<TblEnqPjmfDet> TblEnqPjmfDets { get; set; }
    }
}
