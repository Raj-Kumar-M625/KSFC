using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class FollowUpTaskFilter
    {
        public string Zone { get; set; }
        public string Area { get; set; }
        public string Territory { get; set; }
        public string HQ { get; set; }
        public string TaskDescription { get; set; }
        public string ProjectName { get; set; }
        public string ClientType { get; set; }
        public string ClientName { get; set; }
        public string ActivityType { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        //For Employee Status
        public bool EmployeeStatus { get; set; }
        public string TaskStatus { get; set; }
        public int IsActive { get; set; }
    }
}