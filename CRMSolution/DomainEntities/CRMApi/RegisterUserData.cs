using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class RegisterUserData : EntityRequestBase
    {
        public string IMEI { get; set; }

        //public string EmployeeName { get; set; }

        public string EmployeeCode { get; set; }

        public bool AutoUploadStaus { get; set; }

        public long TenantId { get; set; }
        public long TimeIntervalInMillisecondsForTracking { get; set; }
    }
}
