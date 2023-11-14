using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteDWSData
    {
        public long EntityId { get; set; }
        public string EntityName { get; set; }
        public long AgreementId { get; set; }
        public string Agreement { get; set; }
        public long EntityWorkFlowDetailId { get; set; }
        public string TypeName { get; set; } // crop name
        public string TagName { get; set; }

        public string DWSNumber { get; set; }
        public int BagCount { get; set; }
        public decimal FilledBagsWeightKg { get; set; }
        public decimal EmptyBagsWeightKg { get; set; }

        //public string Notes { get; set; }

        public DateTime TimeStamp { get; set; }
        public string ActivityId { get; set; }
    }
}