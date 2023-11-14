using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class EntityWorkFlow
    {
        public long Id { get; set; }
        public long EntityId { get; set; }
        public string EntityName { get; set; }
        public string TagName { get; set; }
        public string CurrentPhase { get; set; }
        public System.DateTime CurrentPhaseStartDate { get; set; }
        public System.DateTime CurrentPhaseEndDate { get; set; }
        public System.DateTime InitiationDate { get; set; }
        public bool IsComplete { get; set; } // workflow is complete
        public long AgreementId { get; set; }
        public long EntityWorkFlowDetailId { get; set; }
        public string Notes { get; set; }
        public bool IsFollowUpRow { get; set; }
        public bool IsCurrentPhaseRow { get; set; }
        public string Agreement { get; set; }
        public string HQCode { get; set; }

        public string TypeName { get; set; }
        public bool IsActive { get; set; }
    }
}
