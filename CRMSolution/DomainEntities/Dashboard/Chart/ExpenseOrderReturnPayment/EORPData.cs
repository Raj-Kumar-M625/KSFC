using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class EORPData
    {
        public DateTime Date { get; set; }
        public Decimal TotalAmountForDay { get; set; }
        public int TotalItemCountForDay { get; set; }
    }
}
