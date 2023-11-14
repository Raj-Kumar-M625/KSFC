using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SmsSummary
    {
        //public string AreaCode { get; set; }
        //public string AreaName { get; set; }

        // it could be Zone, Area, Territory, HQ Code 
        public string Code { get; set; }
        public string Name { get; set; }

        public int InFieldCount { get; set; }
        public int RegisteredCount { get; set; }
        public int HeadCount { get; set; }

        public decimal TotalOrderAmount { get; set; }
        public decimal TotalPaymentAmount { get; set; }
        public decimal TotalReturnAmount { get; set; }
        public decimal TotalExpenseAmount { get; set; }
        public long TotalActivityCount { get; set; }
    }
}
