using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblEnqPjfinDet
    {
        public int EnqPjfinId { get; set; }
        public int? EnqtempId { get; set; }
        public int? FinyearCode { get; set; }
        public int FincompCd { get; set; }
        public decimal? EnqPjfinamt { get; set; }
        public bool? WhPjprov { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual TblEnqTemptab Enqtemp { get; set; }
        public virtual TblFincompCdtab FincompCdNavigation { get; set; }
        public virtual TblFinyearCdtab FinyearCodeNavigation { get; set; }
    }
}
