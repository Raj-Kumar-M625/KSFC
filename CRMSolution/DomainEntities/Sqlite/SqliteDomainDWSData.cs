using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SqliteDomainDWSData
    {
        public long EntityId { get; set; }
        public string EntityName { get; set; }
        public long AgreementId { get; set; }
        public string Agreement { get; set; }
        public long EntityWorkFlowDetailId { get; set; }
        public string TypeName { get; set; } // crop name
        public string TagName { get; set; }

        public string DWSNumber { get; set; }
        public long BagCount { get; set; }
        public decimal FilledBagsWeightKg { get; set; }
        public decimal EmptyBagsWeightKg { get; set; }

        //public string Notes { get; set; }

        public DateTime TimeStamp { get; set; }
        public string ActivityId { get; set; }
    }

    public class SqliteDWSData : SqliteDomainDWSData
    {
        public long Id { get; set; }
        public long SqliteSTRId { get; set; }

        public bool IsProcessed { get; set; }
        public long DWSId { get; set; }
    }
}
