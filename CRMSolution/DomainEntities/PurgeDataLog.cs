using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class PurgeDataLog
    {
        public long Id { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public long ActionPurged { get; set; }
        public long ActionDupPurged { get; set; }
        public long ExpensePurged { get; set; }
        public long OrderPurged { get; set; }
        public long PaymentPurged { get; set; }
        public long ReturnPurged { get; set; }
        public long EntityPurged { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
