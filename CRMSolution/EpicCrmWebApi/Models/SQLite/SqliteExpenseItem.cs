using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteExpenseItem : SqliteBase
    {
        public string ExpenseType { get; set; } // Phone/Lodge/Travel-Public/Travel-Private/Travel-Company
        // For expense type Travel-Company - amount will be zero
        public decimal Amount { get; set; }
        public string Comment { get; set; }
        public SqliteExpenseItemDetail ExpenseItemDetail { get; set; }
    }
}