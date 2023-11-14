using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class WorkflowSeason
    {
        public long Id { get; set; }
        public string SeasonName { get; set; }
        public string TypeName { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public bool IsOpen { get; set; }
        public string ReferenceId { get; set; }
        public string Description { get; set; }
        public int MaxAgreementsPerEntity { get; set; }
    }
}
