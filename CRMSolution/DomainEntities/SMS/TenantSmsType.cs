using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class TenantSmsType
    {
        public long Id { get; set; }
        public long TenantId { get; set; }
        public string TenantName { get; set; }
        public string TypeName { get; set; }
        public string MessageText { get; set; }
        public string SprocName { get; set; }
        public string SmsProcessClass { get; set; }
        public string PlaceHolders { get; set; }
    }
}
