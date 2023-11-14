using DomainEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class ActivityReportModel
    {
        public long Id { get; set; }

        [Display(Name="Employee Code")]
        public string EmployeeCode { get; set; }

        public string Name { get; set; }
        public DateTime Date { get; set; }

        public long EmployeeDayId { get; set; }
        public long ActivityId { get; set; }

        [Display(Name = "Client Name")]
        public string ClientName { get; set; }

        [Display(Name = "Client Phone")]
        public string ClientPhone { get; set; }

        [Display(Name = "Client Type")]
        public string ClientType { get; set; }

        [Display(Name = "Activity Type")]
        public string ActivityType { get; set; }

        public string Comments { get; set; }
        public DateTime At { get; set; }

        public decimal ActivityDistanceInMeters { get; set; }

        [Display(Name = "Activity Location")]
        public string LocationName { get; set; }

        public decimal TotalDistanceInMeters { get; set; }
        public double Hours { get; set; }  // total hours for the day

        [Display(Name="Day's Distance (Km)")]
        public decimal DayDistanceInKM => TotalDistanceInMeters / 1000.0M;

        [Display(Name = "Activity's Distance (Km)")]
        public decimal ActivityDistanceInKM => ActivityDistanceInMeters / 1000.0M;

        public bool IsStartOrEndDayActivity { get; set; }

        //public IEnumerable<ActivityMapDataModel> ActivityData { get; set; }
    }
}