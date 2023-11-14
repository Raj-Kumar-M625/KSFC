using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblAppUnitAddress
    {
        public int UtAddressRowid { get; set; }
        public int UtCd { get; set; }
        public int? EnqRefNo { get; set; }
        public int AddtypeCd { get; set; }
        public string UtAddress { get; set; }
        public string UtArea { get; set; }
        public string UtCity { get; set; }
        public int? UtPincode { get; set; }
        public int? UtTelephone { get; set; }
        public int? UtMobile { get; set; }
        public int? UtAltMobile { get; set; }
        public string UtEmail { get; set; }
        public string UtAltEmail { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblAppUnitDetail UtCdNavigation { get; set; }
    }
}
