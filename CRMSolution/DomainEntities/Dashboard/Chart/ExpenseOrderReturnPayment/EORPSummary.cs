using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomainEntities
{
    public class EORPSummary
    {
        public decimal OrderTotal { get; set; }
        public decimal OrderAverage { get; set; }
        public decimal ExpenseTotal { get; set; }
        public decimal ExpenseAverage { get; set; }
        public decimal ReturnOrderTotal { get; set; }
        public decimal ReturnOrderAverage { get; set; }
        public decimal PaymentTotal { get; set; }
        public decimal PaymentAverage { get; set; }
    }
}