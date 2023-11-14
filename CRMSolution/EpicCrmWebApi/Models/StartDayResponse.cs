using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace EpicCrmWebApi
{
    public class StartDayResponse : MinimumResponse
    {
        public long TimeIntervalInMillisecondsForTracking { get; set; }
        public long BatchId { get; set; }
        public bool SendLogs { get; set; }
        public bool AutoUpload { get; set; }

        public double MaxDiscountPercentage { get; set; }
        public string DiscountType { get; set; } // Amount / Item

        public int AvailablePaidLeaves { get; set; }
        public int AvailableCompOffs { get; set; }
        public bool ShowAvailableLeaveData { get; set; }
        public string ActivityPageName { get; set; }

        public string WorkflowActivityEntityType { get; set; } // Farmer
        public bool EnhancedDebugEnabled { get; set; }
        public bool TestFeatureEnabled { get; set; }
        public bool VoiceFeatureEnabled { get; set; }

        public string BusinessRole { get; set; }
    }
}