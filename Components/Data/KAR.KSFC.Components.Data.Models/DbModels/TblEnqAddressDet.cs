using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblEnqAddressDet
    {
        public int EnqAddresssId { get; set; }
        public int EnqtempId { get; set; }
        public int AddtypeCd { get; set; }
        public string UniitAddress { get; set; }
        public int? UnitPincode { get; set; }
        public Int64? UnitTelNo { get; set; }
        public Int64? UnitMobileNo { get; set; }
        public string UnitEmail { get; set; }
        public int? UnitFax { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual TblAddressCdtab AddtypeCdNavigation { get; set; }
        public virtual TblEnqTemptab Enqtemp { get; set; }
    }
}
