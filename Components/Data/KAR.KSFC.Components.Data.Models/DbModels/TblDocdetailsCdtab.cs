using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblDocdetailsCdtab
    {
        public TblDocdetailsCdtab()
        {
            TblEnqDocDets = new HashSet<TblEnqDocDet>();
        }

        public int DocdetCd { get; set; }
        public string DocdetDets { get; set; }
        public int DoccatCd { get; set; }
        public decimal? SeqNo { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual TblDoccatCdtab DoccatCdNavigation { get; set; }
        public virtual ICollection<TblEnqDocDet> TblEnqDocDets { get; set; }
    }
}
