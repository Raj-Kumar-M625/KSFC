using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class CRMUsersFilter
    {
        public bool ApplyNameFilter { get; set; }
        public string Name { get; set; }

        public bool ApplyEmployeeCodeFilter { get; set; }
        public string EmployeeCode { get; set; }

        public bool ApplyIMEIFilter { get; set; }
        public string IMEI { get; set; }

        public bool IsActiveInSap { get; set; }
        public bool OnPhone { get; set; }
        public bool ExecAppAccess { get; set; }
        public bool OnWebPortal { get; set; }
        public bool IsEmployeeActive { get; set; }
    }
}
