using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class DownloadWorkFlowFollowUp
    {
        public string TypeName { get; set; }
        public string Phase { get; set; }
        public string PhoneDataEntryPage { get; set; }
        public string FollowUpPhaseTag { get; set; }
        public int MinFollowUps { get; set; }
        public int MaxFollowUps { get; set; }
        public int TargetStartAtDay { get; set; }
        public int TargetEndAtDay { get; set; }
        // May 4 2020
        public int MaxDWS { get; set; }
    }
}