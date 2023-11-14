using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class OtpTab
    {
        public long OtpId { get; set; }
        public string UserId { get; set; }
        public string MobileNo { get; set; }
        public string Otp { get; set; }
        public bool? VerStatus { get; set; }
        public string Process { get; set; }
        public DateTime? Otpexpirationdatetime { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
    }
}
