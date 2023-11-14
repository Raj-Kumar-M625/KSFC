using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class WorkFlowSchedule
    {
        public long Id { get; set; }
        public string TypeName { get; set; } // crop name
        public int Sequence { get; set; }
        public string TagName { get; set; }
        public string Phase { get; set; }
        public int TargetStartAtDay { get; set; }
        public int TargetEndAtDay { get; set; }
        public string PhoneDataEntryPage { get; set; }
    }
}
