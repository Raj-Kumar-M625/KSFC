using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class EmployeeAchieved
    {
        public long Id { get; set; }
        public string EmployeeCode { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string Type { get; set; }
        public decimal AchievedMonthly { get; set; }
    }
}
