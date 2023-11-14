using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblPremCdtab
    {
        public TblPremCdtab()
        {
            TblEnqBasicDets = new HashSet<TblEnqBasicDet>();
        }

        public int PremCd { get; set; }
        public string PremDets { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual ICollection<TblEnqBasicDet> TblEnqBasicDets { get; set; }
    }
}
