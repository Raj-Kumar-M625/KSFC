using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblEnqPromDet
    {
        public int EnqPromId { get; set; }
        public int? EnqtempId { get; set; }
        public long PromoterCode { get; set; }
        public decimal? EnqPromShare { get; set; }
        public int? EnqPromExp { get; set; }
        public string EnqPromExpdet { get; set; }
        public int PdesigCd { get; set; }
        public int DomCd { get; set; }
        public string EnqCibil { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual TblDomiCdtab DomCdNavigation { get; set; }
        public virtual TblEnqTemptab Enqtemp { get; set; }
        public virtual TblPdesigCdtab PdesigCdNavigation { get; set; }
        public virtual TblPromCdtab PromoterCodeNavigation { get; set; }
    }
}
