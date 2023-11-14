using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomainEntities
{
    public class EORPMonth
    {
        // in this Date, day is always 01
        public DateTime Date { get; set; }

        public Decimal ExpenseAmount { get; set; }
        public Decimal OrderAmount { get; set; }
        public Decimal ReturnOrderAmount { get; set; }
        public Decimal PaymentAmount { get; set; }
    }
}