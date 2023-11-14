using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    // used to define/keep associations for a sales Person
    public class OfficeHierarchy : Geography
    {
        public string ZoneName { get; set; }
        public string AreaName { get; set; }
        public string TerritoryName { get; set; }
        public string HQName { get; set; }
        public bool IsZoneSelectable { get; set; } = false;
        public bool IsAreaSelectable { get; set; } = false;
        public bool IsTerritorySelectable { get; set; } = false;
        public bool IsHQSelectable { get; set; } = false;
    }

    public class OfficeHierarchyForAll : OfficeHierarchy
    {
        public string StaffCode { get; set; }
    }
}
