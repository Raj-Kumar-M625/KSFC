using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblFinyearCdtab
    {
        public TblFinyearCdtab()
        {
            TblEnqPjfinDets = new HashSet<TblEnqPjfinDet>();
            TblEnqSfinDets = new HashSet<TblEnqSfinDet>();
        }

        public int FinyearCode { get; set; }
        public string FinyearDesc { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual ICollection<TblEnqPjfinDet> TblEnqPjfinDets { get; set; }
        public virtual ICollection<TblEnqSfinDet> TblEnqSfinDets { get; set; }
    }
}
