using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblTrgEmpGrade
    {
        public TblTrgEmpGrade()
        {
            TblDopCdtabs = new HashSet<TblDopCdtab>();
            TblDophistDets = new HashSet<TblDophistDet>();
            TblEmpchairDets = new HashSet<TblEmpchairDet>();
            TblEmpchairhistDets = new HashSet<TblEmpchairhistDet>();
            TblEmpdesigTabIcDesigCodeNavigations = new HashSet<TblEmpdesigTab>();
            TblEmpdesigTabPpDesignCodeNavigations = new HashSet<TblEmpdesigTab>();
            TblEmpdesigTabSubstDesigCodeNavigations = new HashSet<TblEmpdesigTab>();
            TblEmpdesighistTabs = new HashSet<TblEmpdesighistTab>();
        }

        public string TgesCode { get; set; }
        public string TgesDesc { get; set; }
        public decimal TegsOrder { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual ICollection<TblDopCdtab> TblDopCdtabs { get; set; }
        public virtual ICollection<TblDophistDet> TblDophistDets { get; set; }
        public virtual ICollection<TblEmpchairDet> TblEmpchairDets { get; set; }
        public virtual ICollection<TblEmpchairhistDet> TblEmpchairhistDets { get; set; }
        public virtual ICollection<TblEmpdesigTab> TblEmpdesigTabIcDesigCodeNavigations { get; set; }
        public virtual ICollection<TblEmpdesigTab> TblEmpdesigTabPpDesignCodeNavigations { get; set; }
        public virtual ICollection<TblEmpdesigTab> TblEmpdesigTabSubstDesigCodeNavigations { get; set; }
        public virtual ICollection<TblEmpdesighistTab> TblEmpdesighistTabs { get; set; }
    }
}
