using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblEnqPnwDet
    {
        public int EnqPromnwId { get; set; }
        public int? EnqtempId { get; set; }
        public long PromoterCode { get; set; }
        public decimal? EnqImmov { get; set; }
        public decimal? EnqMov { get; set; }
        public decimal? EnqLiab { get; set; }
        public decimal? EnqNw { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual TblEnqTemptab Enqtemp { get; set; }
        public virtual TblPromCdtab PromoterCodeNavigation { get; set; }
    }
}
