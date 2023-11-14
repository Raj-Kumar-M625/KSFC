using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class UnifiedLogFilter : BaseSearchCriteria
    {
        public string LogType { get; set; }

        public int StartItem { get; set; }
        public int ItemCount { get; set; }

        public bool ApplyProcessFilter { get; set; }
        public string ProcessFilter { get; set; }

        public bool ApplyDateFilter { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
