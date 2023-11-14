using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblTrPjcostCdtab
    {
        public TblTrPjcostCdtab()
        {
            TblEnqTrcostDets = new HashSet<TblEnqTrcostDet>();
        }

        public int TrPjcostCd { get; set; }
        public string TrPjcostDet { get; set; }
        public byte? TrPjcostFlg { get; set; }
        public decimal? SeqNo { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual ICollection<TblEnqTrcostDet> TblEnqTrcostDets { get; set; }
    }
}
