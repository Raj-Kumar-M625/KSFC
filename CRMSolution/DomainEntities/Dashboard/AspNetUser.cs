using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class AspNetUser
    {
        public string UserName { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public int AccessFailedCount { get; set; }
        public DateTime? DisableUserAfterUtc { get; set; }
        public ICollection<string> Roles { get; set; }
    }
}
