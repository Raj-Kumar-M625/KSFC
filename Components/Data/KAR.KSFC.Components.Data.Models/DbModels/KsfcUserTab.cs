using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class KsfcUserTab
    {
        public short UserId { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Password { get; set; }
        public string UserMobile { get; set; }
        public string Gender { get; set; }
        public DateTime? Dateofbirth { get; set; }
        public string Address { get; set; }
        public string Place { get; set; }
        public string Pincode { get; set; }
        public string Pan { get; set; }
        public string Emailid { get; set; }
        public DateTime? Firstappointmentdate { get; set; }
        public bool? Ispasschanged { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
