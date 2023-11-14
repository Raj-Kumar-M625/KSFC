using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblEnqWcDet
    {
        public int EnqWcId { get; set; }
        public int? EnqtempId { get; set; }
        public string EnqWcIfsc { get; set; }
        public string EnqWcBank { get; set; }
        public string EnqWcBranch { get; set; }
        public decimal? EnqWcAmt { get; set; }
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
