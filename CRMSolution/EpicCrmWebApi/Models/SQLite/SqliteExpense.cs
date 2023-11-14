using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteExpense
    {
        // id of the record in Sqlite table
        //public long Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public decimal TotalAmount { get; set; }
        //public int ExpenseItemCount { get; set; }
        public IEnumerable<SqliteExpenseItem> ExpenseItems { get; set; }
    }
}