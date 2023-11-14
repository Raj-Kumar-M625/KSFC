using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class ReturnItemsModel
    {
        public Return Return { get; set; }
        public IEnumerable<ModelReturnItem> Items { get; set; }
    }
}