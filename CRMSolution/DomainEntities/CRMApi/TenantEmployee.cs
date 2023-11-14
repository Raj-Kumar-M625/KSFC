using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class TenantEmployee
    {
        public long Id { get; set; }
        public long TenantId { get; set; }
        public Nullable<long> ManagerId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string EmployeeCode { get; set; }
        public long TimeIntervalInMillisecondsForTrcking { get; set; }
        public string IMEI { get; set; }
        public bool SendLogFromPhone { get; set; }
        public bool AutoUploadFromPhone { get; set; }
        public bool ExecAppAccess { get; set; }
        public double MaxDiscountPercentage { get; set; }
        public string DiscountType { get; set; }
        public string ActivityPageName { get; set; }

        public bool EnhancedDebugEnabled { get; set; }
        public bool TestFeatureEnabled { get; set; }
        public bool VoiceFeatureEnabled { get; set; }
        public int ExecAppDetailLevel { get; set; }
    }
}
