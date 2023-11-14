using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class ActivityMapData
    {
        public long ActivityId { get; set; }
        public long TenantEmployeeId { get; set; }
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
        public int ImageCount { get; set; }
        public int ContactCount { get; set; }
        public int ActivityTrackingType { get; set; }

        public decimal GoogleMapsDistanceInMeters { get; set; }
        public string LocationName { get; set; }

        public bool IsStartOrEndDayActivity { get; set; }
    }
}
