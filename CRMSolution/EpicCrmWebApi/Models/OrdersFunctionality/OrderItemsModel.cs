using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class OrderItemsModel
    {
        //public Order Order { get; set; }
        public ModelOrder Order { get; set; }
        public IEnumerable<ModelOrderItem> Items { get; set; }
    }
}