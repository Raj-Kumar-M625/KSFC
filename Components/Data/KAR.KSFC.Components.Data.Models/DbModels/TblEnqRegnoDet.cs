using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblEnqRegnoDet
    {
        public int EnqRegnoId { get; set; }
        public int? EnqtempId { get; set; }
        public int RegrefCd { get; set; }
        public string EnqRegno { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual TblEnqTemptab Enqtemp { get; set; }
        public virtual TblRegdetailsCdtab RegrefCdNavigation { get; set; }
    }
}
