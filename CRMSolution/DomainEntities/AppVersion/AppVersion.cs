using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class AppVersion
    {
        public long Id { get; set; }
        public string Version { get; set; }
        public string Comment { get; set; }
        public System.DateTime EffectiveDate { get; set; }
        public System.DateTime ExpiryDate { get; set; }
    }
}
