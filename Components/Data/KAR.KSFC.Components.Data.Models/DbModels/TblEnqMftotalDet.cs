using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblEnqMftotalDet
    {
        public int EnqMftotalId { get; set; }
        public int? EnqtempId { get; set; }
        public decimal? EnqEquity { get; set; }
        public decimal? EnqDebt { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual TblEnqTemptab Enqtemp { get; set; }
    }
}
