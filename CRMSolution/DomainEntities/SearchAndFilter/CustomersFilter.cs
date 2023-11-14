using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class CustomersFilter
    {
        public bool ApplyCodeFilter { get; set; }
        public string Code { get; set; }

        public bool ApplyNameFilter { get; set; }
        public string Name { get; set; }

        public bool ApplyTypeFilter { get; set; }
        public string Type { get; set; }

        public bool ApplyHQCodeFilter { get; set; }
        public string HQCode { get; set; }

        public bool ApplyStaffCodeFilter { get; set; }
        public string StaffCode { get; set; }

        public long TenantId { get; set; }
    }
}
