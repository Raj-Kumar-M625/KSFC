using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class ExpenseItem
    {
        public long Id { get; set; }
        public long ExpenseId { get; set; }
        public int SequenceNumber { get; set; }
        public string ExpenseType { get; set; }
        public string TransportType { get; set; }
        public decimal Amount { get; set; }
        public decimal DeductedAmount { get; set; }
        public decimal RevisedAmount { get; set; }
        public long OdometerStart { get; set; }
        public long OdometerEnd { get; set; }
        public int ImageCount { get; set; }
        public string FuelType { get; set; }
        public decimal FuelQuantityInLiters { get; set; }
        public string Comment { get; set; }
    }
}
