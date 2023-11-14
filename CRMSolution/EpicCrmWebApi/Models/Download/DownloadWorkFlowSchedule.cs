using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class DownloadWorkFlowSchedule
    {
        public string TypeName { get; set; } // crop name
        public long Sequence { get; set; }
        public string TagName { get; set; }
        public string Phase { get; set; }
        public int TargetStartAtDay { get; set; }
        public int TargetEndAtDay { get; set; }
        public string PhoneDataEntryPage { get; set; }
    }
}