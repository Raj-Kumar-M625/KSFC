using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SqliteOrderData
    {
        public long Id { get; set; }
        public string PhoneDbId { get; set; }
        public long BatchId { get; set; }
        public long EmployeeId { get; set; }
        public string CustomerCode { get; set; }
        public string OrderType { get; set; }
        public System.DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public long ItemCount { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateUpdated { get; set; }
        public bool IsProcessed { get; set; }
        public string PhoneActivityId { get; set; }
        public long OrderId { get; set; }

        public decimal TotalGST { get; set; }
        public decimal NetAmount { get; set; }
        public decimal MaxDiscountPercentage { get; set; }
        public string DiscountType { get; set; }
        public decimal AppliedDiscountPercentage { get; set; }
        public int ImageCount { get; set; }
    }
}
