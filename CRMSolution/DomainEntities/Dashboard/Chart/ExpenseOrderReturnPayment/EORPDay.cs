using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomainEntities
{
    public class EORPDay
    {
        public DateTime Date { get; set; }

        public Decimal ExpenseAmount { get; set; }
        public Decimal OrderAmount { get; set; }
        public Decimal ReturnOrderAmount { get; set; }
        public Decimal PaymentAmount { get; set; }

        // number of orders
        public int ExpenseCount { get; set; }
        public int OrderCount { get; set; }
        public int ReturnOrderCount { get; set; }
        public int PaymentCount { get; set; }
    }
}