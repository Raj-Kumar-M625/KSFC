using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblEnqDocDet
    {
        public int EnqDocId { get; set; }
        public int? EnqtempId { get; set; }
        public int DoccatCd { get; set; }
        public int DocdetCd { get; set; }
        public string EnqDocPath { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual TblDoccatCdtab DoccatCdNavigation { get; set; }
        public virtual TblDocdetailsCdtab DocdetCdNavigation { get; set; }
        public virtual TblEnqTemptab Enqtemp { get; set; }
    }
}
