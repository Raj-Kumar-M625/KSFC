using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class EORPSummaryResponse
    {
        public decimal OrderTotal { get; set; }
        public decimal OrderAverage { get; set; }
        public decimal ExpenseTotal { get; set; }
        public decimal ExpenseAverage { get; set; }
        public decimal ReturnOrderTotal { get; set; }
        public decimal ReturnOrderAverage { get; set; }
        public decimal PaymentTotal { get; set; }
        public decimal PaymentAverage { get; set; }

        public string OrderTotalDisplay { get; set; }
        public string OrderAverageDisplay { get; set; }
        public string ExpenseTotalDisplay { get; set; }
        public string ExpenseAverageDisplay { get; set; }
        public string ReturnOrderTotalDisplay { get; set; }
        public string ReturnOrderAverageDisplay { get; set; }
        public string PaymentTotalDisplay { get; set; }
        public string PaymentAverageDisplay { get; set; }
    }
}