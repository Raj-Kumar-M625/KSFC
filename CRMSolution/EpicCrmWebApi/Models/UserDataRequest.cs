using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class UserDataRequest : RequestBase
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]{15}$", ErrorMessage = "Invalid IMEI")]
        public string IMEI { get; set; }
    }
}