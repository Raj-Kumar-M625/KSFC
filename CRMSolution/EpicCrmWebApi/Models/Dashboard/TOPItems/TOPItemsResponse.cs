using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class TOPItemsResponse : MinimumResponse
    {
        public IEnumerable<TOPItemsData> Products { get; set; }
        public IEnumerable<TOPItemsData> Returns { get; set; }
        public IEnumerable<TOPItemsData> SalesPersons { get; set; }
        public IEnumerable<TOPItemsData> SalesPersonsByPayments { get; set; }
    }
}