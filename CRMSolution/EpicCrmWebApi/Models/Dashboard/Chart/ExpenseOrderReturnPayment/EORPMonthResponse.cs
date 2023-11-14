using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class EORPMonthResponse
    {
        public String DisplayMonth { get; set; }
        public Decimal OrderAmount { get; set; }
        public Decimal ExpenseAmount { get; set; }
        public Decimal ReturnOrderAmount { get; set; }
        public Decimal PaymentAmount { get; set; }
    }
}