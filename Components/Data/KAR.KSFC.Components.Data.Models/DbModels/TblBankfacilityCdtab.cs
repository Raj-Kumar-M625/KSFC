using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblBankfacilityCdtab
    {
        public TblBankfacilityCdtab()
        {
            TblEnqSisDets = new HashSet<TblEnqSisDet>();
        }

        public int BfacilityCode { get; set; }
        public string BfacilityDesc { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual ICollection<TblEnqSisDet> TblEnqSisDets { get; set; }
    }
}
