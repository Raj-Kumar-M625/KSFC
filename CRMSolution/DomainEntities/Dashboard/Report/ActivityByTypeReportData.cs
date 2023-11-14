using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
   public class ActivityByTypeReportData
    {
        public long DayId { get; set; }
        public DateTime Date { get; set; }
        public long TenantEmployeeId { get; set; }
        public string Name { get; set; }
        public string StaffCode { get; set; }
        public string ClientType { get; set; }
        public string ActivityType { get; set; }
        public int ActivityCount { get; set; }
        public string HQCode { get; set; }
    }
}
