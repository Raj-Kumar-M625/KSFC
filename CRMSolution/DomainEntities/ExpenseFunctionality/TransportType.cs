using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class TransportType
    {
        //public long Id { get; set; }
        public int DisplaySequence { get; set; }
        public string TransportTypeCode { get; set; }
        public decimal ReimbursementRatePerUnit { get; set; }
        public bool IsPublic { get; set; }
    }
}
