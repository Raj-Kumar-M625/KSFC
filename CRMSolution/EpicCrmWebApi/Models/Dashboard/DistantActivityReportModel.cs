using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class DistantActivityReportModel
    {
        public long ActivityId { get; set; }

        [Display(Name = "Activity Date")]
        public System.DateTime ActivityDate { get; set; }

        [Display(Name = "Activity Type")]
        public string ActivityType { get; set; }

        [Display(Name = "Activity At Location?")]
        public bool ActivityAtLocation { get; set; }

        public decimal ActivityLatitude { get; set; }
        public decimal ActivityLongitude { get; set; }

        public long TenantEmployeeId { get; set; }

        [Display(Name = "Employee Code")]
        public string StaffCode { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Display(Name = "Distance (KM)")]
        public decimal RadiusValue { get; set; }

        public long EntityId { get; set; }
    }
}