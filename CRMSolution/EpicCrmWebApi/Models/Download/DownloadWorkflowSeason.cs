using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class DownloadWorkflowSeason
    {
        public long Id { get; set; }
        public string SeasonName { get; set; }
        public string TypeName { get; set; }
        public bool IsOpen { get; set; }
        public int MaxAgreementsPerEntity { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}