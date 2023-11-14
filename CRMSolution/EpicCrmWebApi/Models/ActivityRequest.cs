using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class ActivityRequest : RequestBase
    {
        [Required]
        public long EmployeeId { get; set; }
        [Required]
        public string ClientName { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage ="Invalid Phone Number")]
        public string ClientPhone { get; set; }
        [Required]
        public string ClientType { get; set; }
        [Required]
        public string ActivityType { get; set; }
        [Required]
        [MaxLength(2048, ErrorMessage ="Comment string is too long. Max 2kb allowed.")]
        public string Comments { get; set; }

        [Required]
        public decimal Latitude { get; set; }

        [Required]
        public decimal Longitude { get; set; }
    }
}