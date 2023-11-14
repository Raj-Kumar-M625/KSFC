using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class OfficeHierarchyFilter
    {
        public bool ApplyZoneFilter { get; set; }
        public string Zone { get; set; }

        public bool ApplyAreaFilter { get; set; }
        public string Area { get; set; }

        public bool ApplyTerritoryFilter { get; set; }
        public string Territory { get; set; }
    }
}
