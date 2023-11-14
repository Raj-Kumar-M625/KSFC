using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblEmpdesighistTab
    {
        public int EmpdesighistId { get; set; }
        public string EmpId { get; set; }
        public string DesigCode { get; set; }
        public string DesigTypeCode { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual TblTrgEmpGrade DesigCodeNavigation { get; set; }
        public virtual TblTrgEmployee Emp { get; set; }
    }
}
