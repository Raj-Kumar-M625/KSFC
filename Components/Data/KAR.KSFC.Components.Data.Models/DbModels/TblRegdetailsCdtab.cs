using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblRegdetailsCdtab
    {
        public TblRegdetailsCdtab()
        {
            TblEnqRegnoDets = new HashSet<TblEnqRegnoDet>();
        }

        public int RegrefCd { get; set; }
        public string RegrefDets { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual ICollection<TblEnqRegnoDet> TblEnqRegnoDets { get; set; }
    }
}
