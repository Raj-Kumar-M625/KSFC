using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SalesPersonResult
    {
        public string SalesPersonName { get; set; }
        public string StaffCode { get; set; }
        public bool IsAssigned { get; set; }
        public string AssignedDate { get; set; }
    }
}
