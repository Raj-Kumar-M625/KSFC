using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class DownloadEntityWorkFlow
    {
        public long Id { get; set; }
        public long EntityId { get; set; }
        public string EntityName { get; set; }
        public string TagName { get; set; }
        public string CurrentPhase { get; set; }
        public string CurrentPhaseStartDate { get; set; }
        public string CurrentPhaseEndDate { get; set; }
        public string InitiationDate { get; set; }
        public bool IsComplete { get; set; }
        public long AgreementId { get; set; }
        public long EntityWorkFlowDetailId { get; set; }
        public string Notes { get; set; }
        public bool IsFollowUpRow { get; set; }
        public bool IsCurrentPhaseRow { get; set; }
    }
}