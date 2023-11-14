using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblAddressCdtab
    {
        public TblAddressCdtab()
        {
            TblEnqAddressDets = new HashSet<TblEnqAddressDet>();
        }

        public int AddtypeCd { get; set; }
        public string AddtypeDets { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
        public virtual ICollection<TblEnqAddressDet> TblEnqAddressDets { get; set; }
        public virtual TblIdmUnitAddress TblIdmUnitAddress { get; set; } //Added by GK
    }
}
