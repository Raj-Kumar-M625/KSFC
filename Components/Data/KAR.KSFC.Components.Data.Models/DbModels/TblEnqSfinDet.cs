using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblEnqSfinDet
    {
        public int EnqSisfinId { get; set; }
        public int EnqSisId { get; set; }
        public int? FinyearCode { get; set; }
        public int? FincompCd { get; set; }
        public decimal? EnqFinamt { get; set; }
        public bool? WhProv { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual TblEnqSisDet EnqSis { get; set; }
        public virtual TblFincompCdtab FincompCdNavigation { get; set; }
        public virtual TblFinyearCdtab FinyearCodeNavigation { get; set; }
    }
}
