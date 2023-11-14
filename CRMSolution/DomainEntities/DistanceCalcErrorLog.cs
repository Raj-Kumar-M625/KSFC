using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class DistanceCalcErrorLog
    {
        public long Id { get; set; }
        public string ErrorText { get; set; }
        public string APIName { get; set; }
        public DateTime At { get; set; }

        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }

        public long TrackingId { get; set; }
        public long ActivityId { get; set; }
        public string ActivityType { get; set; }
    }
}
