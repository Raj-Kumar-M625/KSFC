using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class UserDataResponse : MinimumResponse
    {
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public bool IsActive { get; set; }
        public long TimeIntervalInMillisecondsForTracking { get; set; }

        public IEnumerable<CodeTableEx> Locales { get; set; }
    }
}