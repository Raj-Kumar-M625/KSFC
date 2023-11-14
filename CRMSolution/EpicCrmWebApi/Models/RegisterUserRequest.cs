using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class RegisterUserRequest : RequestBase
    {
        [Required]
        //[RegularExpression(@"^[a-zA-Z0-9-]{15,50}$", ErrorMessage = "Invalid IMEI")]
        public string IMEI { get; set; }

        //[Required]
        ////[RegularExpression(@"^[a-zA-Z0-9 ]{1,30}", ErrorMessage = "Invalid User Name")]
        //public string EmployeeName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-z0-9 ]{1,10}", ErrorMessage = "Invalid Employee Code")]
        public string EmployeeCode { get; set; }

        public string PhoneNumber { get; set; }

        public string AppVersion { get; set; }
    }
}