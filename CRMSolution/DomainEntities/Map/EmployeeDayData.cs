using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class EmployeeDayData
    {
        public long EmployeeDayId { get; set; }
        public long EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public string EmployeeName { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
