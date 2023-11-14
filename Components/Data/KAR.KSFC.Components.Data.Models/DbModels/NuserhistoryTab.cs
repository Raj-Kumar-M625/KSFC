using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class NuserhistoryTab
    {
        public long UserHistoryId { get; set; }
        public string VerType { get; set; }
        public string VerDetails { get; set; }
        public string VerStatus { get; set; }
        public DateTime? VerDate { get; set; }
        public long? UserRowId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
        public string Process { get; set; }
    }
}
