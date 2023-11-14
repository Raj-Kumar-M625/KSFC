using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class DownloadAvailableLeaves
    {
        //public long Id { get; set; }
        public string EmployeeCode { get; set; }
        public string LeaveType { get; set; }
        public int TotalLeaves { get; set; }
        public int AvailableLeaves { get; set; }
    }
}
