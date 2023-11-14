using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class AddExpenseDetailRequest
    {
        public long ExpenseTypeId { get; set; }
        public decimal ExpenseAmount { get; set; }
    }
}