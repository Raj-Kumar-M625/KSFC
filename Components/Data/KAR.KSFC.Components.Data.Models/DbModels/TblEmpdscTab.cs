using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblEmpdscTab
    {
        public int EmpuserId { get; set; }
        public string EmpId { get; set; }
        public string EmpPswd { get; set; }
        public int? DscSlno { get; set; }
        public string DscPubkey { get; set; }
        public DateTime? DscExpdate { get; set; }
        public string DscName { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsPswdChng { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual TblTrgEmployee Emp { get; set; }
    }
}
