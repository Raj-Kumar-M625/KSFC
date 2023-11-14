using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomainEntities
{
    public class EORP
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DayCount { get; set; }

        public EORPSummary EORPSummary { get; set; }
        public IEnumerable<EORPMonth> EORPMonthlySummary { get; set; }
        public IEnumerable<EORPDay> EORPDays { get; set; }
    }
}