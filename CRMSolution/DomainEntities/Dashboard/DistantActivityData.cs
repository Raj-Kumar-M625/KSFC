using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class DistantActivityData
    {
        public long ActivityId { get; set; }
        public System.DateTime ActivityDate { get; set; }
        public string ActivityType { get; set; }
        public bool ActivityAtLocation { get; set; }
        public decimal ActivityLatitude { get; set; }
        public decimal ActivityLongitude { get; set; }

        public long TenantEmployeeId { get; set; }
        public string StaffCode { get; set; }
        public string EmployeeName { get; set; }

        public long EntityId { get; set; }

        public decimal RadiusValue { get; set; }

        public bool Delete { get; set; } 
    }
}
