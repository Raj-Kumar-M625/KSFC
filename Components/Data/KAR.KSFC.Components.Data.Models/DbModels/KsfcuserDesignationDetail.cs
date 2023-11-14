using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class KsfcuserDesignationDetail
    {
        public long? UserRowid { get; set; }
        public string Employeeid { get; set; }
        public long? SubstantiveDesigCode { get; set; }
        public DateTime? SubstantiveDesigDate { get; set; }
        public long? PpDesigCode { get; set; }
        public DateTime? PpDesigDate { get; set; }
        public long? InchargeDesigCode { get; set; }
        public DateTime? InchargeDesigDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
    }
}
