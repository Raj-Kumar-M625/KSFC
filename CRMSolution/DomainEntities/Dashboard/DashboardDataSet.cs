using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class DashboardDataSet : TrackingData
    {
        public DateTime Date { get; set; }
        public long TenantEmployeeId { get; set; }
        public long EmployeeDayId { get; set; }
        public decimal TotalDistanceInMeters { get; set; }
        public int ActivityCount { get; set; }
    }
}
