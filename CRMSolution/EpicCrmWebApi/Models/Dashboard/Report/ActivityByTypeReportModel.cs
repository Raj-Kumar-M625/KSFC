using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EpicCrmWebApi
{
    public class ActivityByTypeReportModel
    {
        [Display(Name = "Employee Name")]
        public string Name { get; set; }

        [Display(Name = "Employee Code")]
        public string StaffCode { get; set; }

        [Display(Name = "Client Type")]
        public string ClientType { get; set; }

        [Display(Name = "Activity Type")]
        public string ActivityType { get; set; }

        public DateTime Date { get; set; }

        [Display(Name = "Activity Count")]
        public int ActivityCount { get; set; }

        [Display(Name = "Area") ]
        public string AreaName { get; set; }
    }
}