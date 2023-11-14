using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicCrmWebApi
{
    /// <summary>
    /// Class used to Create a new Expense
    /// </summary>
    public class AddExpenseRequest
    {
        public long EmployeeId { get; set; }
        public DateTime ExpenseDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string PlaceForm { get; set; }
        public string PlaceTo { get; set; }
        public string TravelMode { get; set; }
        public long OdometerStart { get; set; }
        public long OdometerEnd { get; set; }

        public IEnumerable<long> AttachedActivities { get; set; }
        public IEnumerable<AddExpenseDetailRequest> ExpenseDetails { get; set; }
    }
}
