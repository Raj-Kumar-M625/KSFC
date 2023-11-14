using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class ConsolidatedCustomerData
    {
        public int CustomerCount { get; set; }
        public decimal Outstanding { get; set; }
        public decimal LongOutstanding { get; set; }
        public decimal Target { get; set; }
        public decimal Sales { get; set; }
        public decimal Payment { get; set; }
    }
}
