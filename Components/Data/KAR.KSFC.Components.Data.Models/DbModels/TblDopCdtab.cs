using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblDopCdtab
    {
        public int DopId { get; set; }
        public string TgesCode { get; set; }
        public int RoleId { get; set; }
        public int AttrId { get; set; }
        public int SubattrId { get; set; }
        public int DopValue { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual TblAttrCdtab Attr { get; set; }
        public virtual TblRoleCdtab Role { get; set; }
        public virtual TblSubattrCdtab Subattr { get; set; }
        public virtual TblTrgEmpGrade TgesCodeNavigation { get; set; }
    }
}
