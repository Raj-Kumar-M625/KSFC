using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SqliteExpenseData
    {
        public long Id { get; set; }
        public long SqliteTableId { get; set; }
        public long BatchId { get; set; }
        public long EmployeeId { get; set; }
        public string ExpenseType { get; set; }
        public decimal Amount { get; set; }
        public long OdometerStart { get; set; }
        public long OdometerEnd { get; set; }
        public string VehicleType { get; set; }
        public int ImageCount { get; set; }
        public bool IsProcessed { get; set; }
        public long ExpenseItemId { get; set; }
        public string FuelType { get; set; }
        public Decimal FuelQuantityInLiters { get; set; }
        public string Comment { get; set; }
    }
}
