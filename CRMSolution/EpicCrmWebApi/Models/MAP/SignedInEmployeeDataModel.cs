using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SignedInEmployeeDataModel
    {
        public long TrackingRecordId { get; set; }
        public System.DateTime TrackingTime { get; set; }
        public long EmployeeId { get; set; }
        public long EmployeeDayId { get; set; }
        public string EmployeeName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}