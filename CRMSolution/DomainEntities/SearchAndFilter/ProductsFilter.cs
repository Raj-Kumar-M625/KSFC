using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class ProductsFilter
    {
        public bool ApplyProductCodeFilter { get; set; }
        public string ProductCode { get; set; }

        public bool ApplyNameFilter { get; set; }
        public string Name { get; set; }

        public bool ApplyStatusFilter { get; set; }
        public bool Status { get; set; }

        public bool ApplyZoneFilter { get; set; }
        public string Zone { get; set; }

        public bool ApplyAreaFilter { get; set; }
        public string Area { get; set; }

        public int MaxResultCount { get; set; }
        //public IEnumerable<string> FilteringAreaCodes { get; set; }

        public bool IsSuperAdmin { get; set; }
        public string StaffCode { get; set; }
    }
}
