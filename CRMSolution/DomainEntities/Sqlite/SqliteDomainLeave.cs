using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SqliteDomainLeave
    {
        public string PhoneDbId { get; set; }
        public bool IsHalfDayLeave { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string LeaveType { get; set; }
        public string LeaveReason { get; set; }
        public string Comment { get; set; }
        public int DaysCountExcludingHolidays { get; set; }
        public int DaysCount { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
