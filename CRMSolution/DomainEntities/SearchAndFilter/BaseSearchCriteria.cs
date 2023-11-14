using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class BaseSearchCriteria
    {
        public bool ApplyZoneFilter { get; set; }
        public string Zone { get; set; }

        public bool ApplyAreaFilter { get; set; }
        public string Area { get; set; }

        public bool ApplyTerritoryFilter { get; set; }
        public string Territory { get; set; }

        public bool ApplyHQFilter { get; set; }
        public String HQ { get; set; }

        /// <summary>
        /// used to apply security
        /// </summary>
        public bool IsSuperAdmin { get; set; }

        /// <summary>
        /// used to apply security
        /// </summary>
        public string CurrentUserStaffCode { get; set; }

        public DateTime CurrentISTTime { get; set; }

        public long TenantId { get; set; }
    }
}
