using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class EORPResponse : MinimumResponse
    {
        public DateTime Startdate { get; set; }
        public DateTime EndDate { get; set; }
        public int DayCount { get; set; }

        public EORPSummaryResponse EORPSummary { get; set; }
        public IEnumerable<EORPDayResponse> EORPDays { get; set; }
    }
}