using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SqliteDomainExpense
    {
        // id of the record in Sqlite table
        public long Id { get; set; }
        public DateTime? TimeStamp { get; set; }
        public decimal TotalAmount { get; set; }
        public int ExpenseItemCount { get; set; }
        public IEnumerable<SqliteDomainExpenseItem> ExpenseItems { get; set; }
    }
}
