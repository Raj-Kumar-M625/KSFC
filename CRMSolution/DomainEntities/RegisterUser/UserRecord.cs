using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class UserRecord
    {
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public bool IsActive { get; set; }
        public string Message { get; set; }
        public long TimeIntervalInMillisecondsForTracking { get; set; }
    }
}
