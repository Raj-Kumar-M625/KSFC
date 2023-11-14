using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class ActivityMapDataModel
    {
        public long ActivityId { get; set; }
        public long EmployeeDayId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string ClientName { get; set; }
        public string ClientType { get; set; }
        public bool AtLocation { get; set; }
        public string ActivityType { get; set; }
        public decimal ActivityAmount { get; set; }
        public string Comments { get; set; }
        public System.DateTime At { get; set; }
        public string ClientPhone { get; set; }

        [Display(Name="Images?")]
        public int ImageCount { get; set; }
        [Display(Name = "Contacts?")]
        public int ContactCount { get; set; }

        public int ActivityTrackingType { get; set; }
    }
}