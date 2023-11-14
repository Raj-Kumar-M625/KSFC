using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblIndCdtab
    {
        public TblIndCdtab()
        {
            TblProdCdtabs = new HashSet<TblProdCdtab>();
            UnitInfo1s = new HashSet<UnitInfo1>();
        }

        public int IndCd { get; set; }
        public string IndDets { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual ICollection<TblProdCdtab> TblProdCdtabs { get; set; }
        public virtual ICollection<UnitInfo1> UnitInfo1s { get; set; }

        public virtual IdmUnitProducts IdmUnitProducts { get; set; }
        public virtual TblAppUnitDetail UtCdNavigation { get; set; }
    }
}
