using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class DscDetailstab
    {
        public short? Id { get; set; }
        public long? DscrowId { get; set; }
        public string EmpId { get; set; }
        public string EmpPassword { get; set; }
        public string DscSerno { get; set; }
        public string DscPublicKey { get; set; }
        public DateTime? DscExpirtDate { get; set; }
        public string DscName { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
    }
}
