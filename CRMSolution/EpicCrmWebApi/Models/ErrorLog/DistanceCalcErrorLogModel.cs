using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class DistanceCalcErrorLogModel
    {
        public long Id { get; set; }
        public string ErrorText { get; set; }
        public string APIName { get; set; }
        [Display(Name="Tracking Table At")]
        public DateTime At { get; set; }

        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }

        public long TrackingId { get; set; }
        public long ActivityId { get; set; }
        public string ActivityType { get; set; }
    }
}