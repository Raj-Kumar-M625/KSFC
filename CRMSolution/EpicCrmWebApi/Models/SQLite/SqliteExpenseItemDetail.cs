using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteExpenseItemDetail
    {
        public long OdometerStart { get; set; }
        public long OdometerEnd { get; set; }  // not used when Expense Type is Fuel
        // Vehicle Type is applicable only when Expense Type is Travel-Public / Travel-Private / Travel-Company / Fuel
        // For ExpenseType Travel-Public > Bus/Train/Flight/Auto etc
        // For ExpenseType Travel-Private > Two-Wheelher / Four-Wheelher
        // For ExpenseType Travel-Company > Two-Wheelher / Four-Wheelher
        // For ExpenseType Fuel > Two-Wheelher / Four-Wheelher
        public string VehicleType { get; set; }

        public string FuelType { get; set; }
        public decimal FuelQuantityInLiters { get; set; }
    }
}