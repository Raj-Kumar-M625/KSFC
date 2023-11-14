using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class EmployeeRecord
    {
        public long EmployeeId { get; set; }
        public long TenantId { get; set; }
        public Nullable<long> ManagerId { get; set; }
        public string Name { get; set; }
        public string EmployeeCode { get; set; }
        public bool IsActive { get; set; }
        public string IMEI { get; set; }
        public bool OnWebPortal { get; set; }
        public string ExpenseHQCode { get; set; }

        public bool IsActiveInSap { get; set; }
        public string Phone { get; set; }

        public bool SendLogFromPhone { get; set; }
        public bool ExecAppAccess { get; set; }

        public bool AutoUploadFromPhone { get; set; }
        public string ActivityPageName { get; set; }
        public string LocationFromType { get; set; }

        public bool EnhancedDebugEnabled { get; set; }

        public bool TestFeatureEnabled { get; set; }

        public bool VoiceFeatureEnabled { get; set; }
        public int ExecAppDetailLevel { get; set; }
    }
}
