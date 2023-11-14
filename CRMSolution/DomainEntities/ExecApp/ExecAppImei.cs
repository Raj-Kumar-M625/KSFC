using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class ExecAppImei
    {
        public long Id { get; set; }
        public string IMEINumber { get; set; }
        public string Comment { get; set; }
        public System.DateTime EffectiveDate { get; set; }
        public System.DateTime ExpiryDate { get; set; }
        public bool IsSupportPerson { get; set; }
        public bool EnableLog { get; set; }
        public int ExecAppDetailLevel { get; set; }
    }
}
