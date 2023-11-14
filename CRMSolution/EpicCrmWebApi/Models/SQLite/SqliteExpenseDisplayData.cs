using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteExpenseDisplayData
    {
        public long Id { get; set; }

        [Display(Name = "Mob Table Id")]
        public long SqliteTableId { get; set; }
        public long BatchId { get; set; }
        public long EmployeeId { get; set; }

        [Display(Name = "Expense Type")]
        public string ExpenseType { get; set; }


        public decimal Amount { get; set; }

        [Display(Name = "Odometer")]
        public long OdometerStart { get; set; }
        public long OdometerEnd { get; set; }

        public string Odometer
        {
            get
            {
                if (OdometerStart > 0 && OdometerEnd > 0)
                {
                    return $"{OdometerStart}-{OdometerEnd}";
                }

                if (OdometerStart > 0)
                {
                    return $"{OdometerStart}";
                }

                if (OdometerEnd > 0)
                {
                    return $"{OdometerStart}-{OdometerEnd}";
                }

                return "";
            }
        }

        [Display(Name = "Vehicle Type")]
        public string VehicleType { get; set; }
        [Display(Name = "Image?")]
        public int ImageCount { get; set; }

        [Display(Name = "Processed?")]
        public bool IsProcessed { get; set; }
        public long ExpenseItemId { get; set; }

        [Display(Name = "Fuel Type")]
        public string FuelType { get; set; }
        [Display(Name = "Fuel (ltr)")]
        public Decimal FuelQuantityInLiters { get; set; }

        [Display(Name = "Notes")]
        public string Comment { get; set; }
    }
}