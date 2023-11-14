using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SqliteActionProcessLog
    {
        public long Id { get; set; }
        public long TenantId { get; set; }
        public long EmployeeId { get; set; }

        public bool LockAcquiredStatus { get; set; }
        public System.DateTime At { get; set; }
        public System.DateTime Timestamp { get; set; }

        public bool HasCompleted { get; set; }
        public bool HasFailed { get; set; }

        public string ProcessName { get; set; }
    }
}
