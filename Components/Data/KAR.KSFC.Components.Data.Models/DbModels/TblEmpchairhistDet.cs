using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblEmpchairhistDet
    {
        public int EmpchairhistId { get; set; }
        public string EmpId { get; set; }
        public byte OffcCd { get; set; }
        public string TgesCode { get; set; }
        public int ChairCode { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual TblChairCdtab ChairCodeNavigation { get; set; }
        public virtual TblTrgEmployee Emp { get; set; }
        public virtual OffcCdtab OffcCdNavigation { get; set; }
        public virtual TblTrgEmpGrade TgesCodeNavigation { get; set; }
    }
}
