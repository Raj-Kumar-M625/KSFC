using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class ActivityRecord
    {
        public long Id { get; set; }
        public long EmployeeDayId { get; set; }

        public string ClientName { get; set; }

        public string ClientPhone { get; set; }

        public string ClientType { get; set; }

        public string ActivityType { get; set; }

        public string Comments { get; set; }

        public System.DateTime At { get; set; }
    }
}
