using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class TenantWorkDay
    {
        public int Id { get; set; }
        public long TenantId { get; set; }
        public string TenantName { get; set; }

        public string WeekDayName { get; set; }
        public bool IsWorkingDay { get; set; }
    }
}
