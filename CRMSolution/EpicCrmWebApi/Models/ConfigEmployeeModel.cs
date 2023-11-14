using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class ConfigEmployeeModel
    {
        [Required]
        [RegularExpression(@"^[0-9a-zA-Z]{4,10}$", ErrorMessage = "Invalid Employee Code")]
        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }
        public long EmployeeId { get; set; }
        public bool AutoUploadFromPhone { get; set; }
        public string ActivityPageName { get; set; }
        public string LocationFromType { get; set; }

        public bool EnhancedDebugEnabled { get; set; }

        public bool TestFeatureEnabled { get; set; }
        public bool VoiceFeatureEnabled { get; set; }
        public int ExecAppDetailLevel { get; set; }
    }
}