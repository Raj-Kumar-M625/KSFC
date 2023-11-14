using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class ProjectsFilter
    {
        public bool ApplyNameFilter { get; set; }
        public string Name { get; set; }

        public bool ApplyCategoryFilter { get; set; }
        public string Category { get; set; }

        public bool ApplyStatusFilter { get; set; }
        public string Status { get; set; }

        public bool ApplyDateFilter { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public bool ApplyActiveFilter { get; set; }
        public bool IsActive { get; set; }

        public long TenantId { get; set; }

    }
}
