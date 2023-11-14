using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class WorkFlowFollowUp
    {
        public long Id { get; set; }
        public string TypeName { get; set; }
        public string Phase { get; set; }
        public string PhoneDataEntryPage { get; set; }
        public string FollowUpPhaseTag { get; set; }
        public int MinFollowUps { get; set; }
        public int MaxFollowUps { get; set; }
        public int TargetStartAtDay { get; set; }
        public int TargetEndAtDay { get; set; }
        public bool IsActive { get; set; }

        // May 4 2020
        public int MaxDWS { get; set; }
    }
}
