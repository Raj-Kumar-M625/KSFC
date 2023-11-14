using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class TOPItemsSummary
    {
        public IEnumerable<TOPItemsData> Products { get; set; }
        public IEnumerable<TOPItemsData> Returns { get; set; }
        public IEnumerable<TOPItemsData> SalesPersonsByOrders { get; set; }
        public IEnumerable<TOPItemsData> SalesPersonsByPayments { get; set; }
    }
}
