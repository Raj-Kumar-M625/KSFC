using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class ConfigEmployeeModel
    {
        public long EmployeeId { get; set; }
        public bool AutoUploadFromPhone { get; set; }
        public string ActivityPageName { get; set; }
        public string LocationFromType { get; set; }
        public bool EnhancedDebugEnabled { get; set; }
        public bool TestFeatureEnabled { get; set; }
        public bool VoiceFeatureEnabled { get; set; }
        public int ExecAppDetailLevel { get; set; }
    }
}
