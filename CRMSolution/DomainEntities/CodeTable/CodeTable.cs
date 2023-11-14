using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class CodeTable
    {
        public string Code { get; set; }
        public int DisplaySequence { get; set; }
    }

    public class CodeTableEx : CodeTable
    {
        public string CodeName { get; set; }
    }
}
