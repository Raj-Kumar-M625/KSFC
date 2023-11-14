using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
  public   class LeaveTypes
    {
        public long Id { get; set; }
        public string EmployeeCode { get; set; }
        public string LeaveType { get; set; }
        public int TotalLeaves { get; set; }
        //public System.DateTime StartDate { get; set; }
        //public System.DateTime EndDate { get; set; }
    }
}
