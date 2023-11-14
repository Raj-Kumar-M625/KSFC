using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteReturnOrder
    {
        public string Id { get; set; }
        public string CustomerCode { get; set; }  // Customer to whom this order/return belongs
        public DateTime TimeStamp { get; set; }  // only date part is relevant - as order return date
        public decimal TotalAmount { get; set; } 
        public string ReferenceNum { get; set; }
        public string Comment { get; set; }
        public IEnumerable<SqliteReturnOrderItem> Items { get; set; }
        public string ActivityId { get; set; }
    }
}