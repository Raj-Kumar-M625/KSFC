using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblEmpdesigTab
    {
        public int EmpdesigId { get; set; }
        public string EmpId { get; set; }
        public string SubstDesigCode { get; set; }
        public DateTime? SubstDate { get; set; }
        public string PpDesignCode { get; set; }
        public DateTime? PpDate { get; set; }
        public string IcDesigCode { get; set; }
        public DateTime? IcDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual TblTrgEmployee Emp { get; set; }
        public virtual TblTrgEmpGrade IcDesigCodeNavigation { get; set; }
        public virtual TblTrgEmpGrade PpDesignCodeNavigation { get; set; }
        public virtual TblTrgEmpGrade SubstDesigCodeNavigation { get; set; }
    }
}
