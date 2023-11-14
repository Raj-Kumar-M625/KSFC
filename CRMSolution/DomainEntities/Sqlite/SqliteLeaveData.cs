using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SqliteLeaveData
    {
        public long Id { get; set; }
        public long BatchId { get; set; }
        public long EmployeeId { get; set; }
        public string PhoneDbId { get; set; }
        public bool IsHalfDayLeave { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public string LeaveType { get; set; }
        public string LeaveReason { get; set; }
        public string Comment { get; set; }
        public bool IsProcessed { get; set; }
        public long LeaveId { get; set; }
        public int DaysCountExcludingHolidays { get; set; }
        public int DaysCount { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateUpdated { get; set; }
    }
}
