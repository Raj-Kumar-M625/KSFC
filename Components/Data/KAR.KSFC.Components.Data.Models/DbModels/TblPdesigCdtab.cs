using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblPdesigCdtab
    {
        public TblPdesigCdtab()
        {
            TblEnqPromDets = new HashSet<TblEnqPromDet>();
        }

        public int PdesigCd { get; set; }
        public string PdesigDets { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual ICollection<TblEnqPromDet> TblEnqPromDets { get; set; }
        public virtual IdmPromoter IdmPromoter { get; set; }

        
    }
}
