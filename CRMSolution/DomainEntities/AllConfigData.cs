using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class AllConfigData 
    {
        public SiteConfigData SiteInfo { get; set; }
        public ICollection<GlobalConfigData> GlobalConfig { get; set; }
        public DBServer DbServer { get; set; }
    }
}
