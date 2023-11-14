using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class TenantSmsData
    {
        public long Id { get; set; }
        public long TenantId { get; set; }
        public string TypeName { get; set; }
        public string DataType { get; set; }
        public string MessageData { get; set; }
        public bool IsSent { get; set; }
        public bool IsFailed { get; set; }
        public string FailedText { get; set; }
        public Nullable<System.DateTime> LockTimestamp { get; set; }
    }
}
