using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class STRTag
    {
        public long Id { get; set; }
        public string STRNumber { get; set; }
        public DateTime STRDate { get; set; }

        public long? DWSCount { get; set; }
        public long? BagCount { get; set; }

        public decimal? GrossWeight { get; set; }
        public decimal? NetWeight { get; set; }

        public long? StartOdo { get; set; }
        public long? STRCount { get; set; }

        public string Status { get; set; }

        public long CyclicCount { get; set; }

        public bool IsFinal { get; set; }

        public bool IsCancel { get; set; }

        public string CurrentUser { get; set; }
        public long STRWeightRecId { get; set; } = 0;
        public long STRWeightCyclicCount { get; set; } = 0;

        public bool IsEditAllowed => Status.Equals(STRDWSStatus.Pending.ToString(), StringComparison.OrdinalIgnoreCase);
    }
}
