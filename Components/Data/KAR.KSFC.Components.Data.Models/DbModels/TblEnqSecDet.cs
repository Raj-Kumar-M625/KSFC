using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblEnqSecDet
    {
        public int EnqSecId { get; set; }
        public int? EnqtempId { get; set; }
        public int? SecCode { get; set; }
        public short SecCd { get; set; }
        public string EnqSecDesc { get; set; }
        public decimal? EnqSecValue { get; set; }
        public string EnqSecName { get; set; }
        public int PromrelCd { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual TblEnqTemptab Enqtemp { get; set; }
        public virtual TblPromrelCdtab PromrelCdNavigation { get; set; }
        public virtual TblSecCdtab SecCdNavigation { get; set; }
        public virtual TblPjsecCdtab SecCodeNavigation { get; set; }
    }
}
