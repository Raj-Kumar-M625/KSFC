using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SqliteDomainExpenseItem
    {
        public string ExpenseType { get; set; } // Phone/Lodge/Travel-Public/Travel-Private
        public decimal Amount { get; set; }
        public long OdometerStart { get; set; }
        public long OdometerEnd { get; set; }
        // Vehicle Type is applicable only when Expense Type is Travel-Public / Travel-Private
        // For ExpenseType Travel-Public > Bus/Train/Flight/Auto etc
        // For ExpenseType Travel-Private > Two-Wheelher / Four-Wheelher
        public string VehicleType { get; set; }
        public string FuelType { get; set; }
        public decimal FuelQuantityInLiters { get; set; }
        public string Comment { get; set; }
        //public IEnumerable<byte[]> Images { get; set; }
        public IEnumerable<string> Images { get; set; }
    }
}
