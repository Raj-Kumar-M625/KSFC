using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblAppTeamDet
    {
        public int AppTeamRowid { get; set; }
        public int UtCd { get; set; }
        public int? EnqRefNo { get; set; }
        public string EmpId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblAppUnitDetail UtCdNavigation { get; set; }
    }
}
