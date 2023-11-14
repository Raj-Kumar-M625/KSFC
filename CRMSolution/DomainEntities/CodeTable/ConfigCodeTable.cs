using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class ConfigCodeTable
    {
        public long Id { get; set; }
       
        public string CodeType { get; set; }
        public bool ApplyCodeTypeFilter { get; set; }

        public string CodeValue { get; set; }
        public int DisplaySequence { get; set; }
        public bool IsActive { get; set; }

        public string CodeName { get; set; }
        public bool ApplyCodeNameFilter { get; set; }

        public long TenantId { get; set; }

        public bool CodeStatus { get; set; }
        public bool ApplyCodeStatusFilter { get; set; }

        public List<string> UniqueCodeTypes { get; set; }
    }
}
