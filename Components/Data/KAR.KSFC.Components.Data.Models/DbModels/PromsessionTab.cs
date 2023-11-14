using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class PromsessionTab
    {
        public int PromsessionId { get; set; }
        public string PromPan { get; set; }
        public DateTime? LoginDateTime { get; set; }
        public DateTime? LogoutDateTime { get; set; }
        public string Ipadress { get; set; }
        public string Accesstoken { get; set; }
        public string Refreshtoken { get; set; }
        public bool? Accesstokenrevoke { get; set; }
        public bool? Refreshtokenrevoke { get; set; }
        public DateTime? Accesstokenexpirydatetime { get; set; }
        public DateTime? Refreshtokenexpirydatetime { get; set; }
        public bool? SessionStatus { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
    }
}
