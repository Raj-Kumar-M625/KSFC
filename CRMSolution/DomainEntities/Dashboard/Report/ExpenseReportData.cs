using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class ExpenseData
    {
        public long EmployeeId { get; set; }
        public long DayId { get; set; }
        public string ExpenseType { get; set; }
        public string TransportType { get; set; }
        public decimal Amount { get; set; }
        public long OdometerStart { get; set; }
        public long OdometerEnd { get; set; }
        public int ImageCount { get; set; }
        public string FuelType { get; set; }
        public decimal FuelQuantityInLiters { get; set; }
    }

    public class ExpenseReportData
    {
        public long EmployeeId { get; set; }
        public long DayId { get; set; }

        public string Name { get; set; } = "";
        public DateTime ExpenseDate { get; set; }
        public string ExpenseHQCode { get; set; }

        public EmployeeDailyConsolidation DailyConsolidation { get; set; }

        //public decimal RepairAmount { get; set; }
        public decimal DailyAllowanceAmount { get; set; }
        public decimal DALocalAmount { get; set; }
        public decimal DAOutstationAmount { get; set; }

        public decimal TelephoneAmount { get; set; }
        public decimal InternetAmount { get; set; }
        public decimal LodgeRent { get; set; }
        public decimal VehicleRepairAmount { get; set; }
        public decimal OwnStayAmount { get; set; }
        public decimal TollTaxAmount { get; set; }
        public decimal ParkingAmount { get; set; }
        public decimal DriverSalary { get; set; }
        public decimal StationaryAmount { get; set; }
        public decimal MiscellaneousAmount { get; set; }
        public IEnumerable<TravelPublicExpenseData> TravelPublic { get; set; }
        public IEnumerable<TravelPrivateExpenseData> TravelPrivate { get; set; }
        public IEnumerable<TravelCompanyExpenseData> TravelCompany { get; set; }
        public IEnumerable<FuelExpenseData> Fuel { get; set; }
    }

    public class TravelPublicExpenseData
    {
        public string TransportType { get; set; }
        public decimal Amount { get; set; }
    }

    public class TravelPrivateExpenseData
    {
        public string TransportType { get; set; }
        public long OdometerStart { get; set; }
        public long OdometerEnd { get; set; }
        public decimal Amount { get; set; }
    }

    public class TravelCompanyExpenseData
    {
        public long OdometerStart { get; set; }
        public long OdometerEnd { get; set; }
    }

    public class FuelExpenseData
    {
        public decimal Amount { get; set; }
        public string FuelType { get; set; }
        public decimal FuelQuantityInLiters { get; set; }
    }
}
