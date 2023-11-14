using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class PPA
    {
        //public long Id { get; set; }
        //public long TenantId { get; set; }
        public string AreaCode { get; set; }
        public string StaffCode { get; set; }
        public string PPACode { get; set; }
        public string PPAName { get; set; }
        public string PPAContact { get; set; }
        public string Location { get; set; }
    }
}
