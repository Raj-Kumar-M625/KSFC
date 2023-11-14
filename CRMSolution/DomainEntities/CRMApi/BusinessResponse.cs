using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class BusinessResponse
    {
        public long EmployeeDayId { get; set; }
        public string Content { get; set; }
        public long TimeIntervalInMillisecondsForTracking { get; set; }
        public BusinessResponse()
        {
            TimeIntervalInMillisecondsForTracking = 0;
        }
    }
}
