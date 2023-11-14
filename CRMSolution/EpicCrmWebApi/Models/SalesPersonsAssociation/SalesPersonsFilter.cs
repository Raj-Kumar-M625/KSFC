using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SalesPersonsFilter
    {
        public IEnumerable<DomainEntities.SalesPerson> SalesPerson { get; set; }
    }
}