using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblEnqSisDet
    {
        public TblEnqSisDet()
        {
            TblEnqSfinDets = new HashSet<TblEnqSfinDet>();
        }

        public int EnqSisId { get; set; }
        public int? EnqtempId { get; set; }
        public string EnqSisName { get; set; }
        public string EnqSisIfsc { get; set; }
        public int? BfacilityCode { get; set; }
        public decimal? EnqOutamt { get; set; }
        public decimal? EnqDeftamt { get; set; }
        public decimal? EnqOts { get; set; }
        public string EnqRelief { get; set; }
        public string EnqSiscibil { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual TblBankfacilityCdtab BfacilityCodeNavigation { get; set; }
        public virtual TblEnqTemptab Enqtemp { get; set; }
        public virtual ICollection<TblEnqSfinDet> TblEnqSfinDets { get; set; }
    }
}
