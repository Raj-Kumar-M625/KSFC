using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteOrder : SqliteBase
    {
        public string Id { get; set; }  // ID of the order in Sqlite table
        public string CustomerCode { get; set; }  // Customer to whom this order/return belongs
        public DateTime TimeStamp { get; set; }  // only date part is relevant - as order date
        public decimal TotalAmount { get; set; } // total amount of order - for returns it will be 0
                    // this is discounted Price * qty

        public IEnumerable<SqliteOrderItem> Items { get; set; }
        public string ActivityId { get; set; } // guid of Activity Id, as in Realm DB

        public decimal MaxDiscountPercentage { get; set; }
        public string DiscountType { get; set; }
        public decimal AppliedDiscountPercentage { get; set; }
        public decimal TotalGST { get; set; }
        public decimal NetAmount { get; set; }
    }
}