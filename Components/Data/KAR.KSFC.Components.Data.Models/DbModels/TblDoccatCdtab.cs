using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblDoccatCdtab
    {
        public TblDoccatCdtab()
        {
            TblDocdetailsCdtabs = new HashSet<TblDocdetailsCdtab>();
            TblEnqDocDets = new HashSet<TblEnqDocDet>();
        }

        public int DoccatCd { get; set; }
        public string DoccatDets { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual ICollection<TblDocdetailsCdtab> TblDocdetailsCdtabs { get; set; }
        public virtual ICollection<TblEnqDocDet> TblEnqDocDets { get; set; }
    }
}
