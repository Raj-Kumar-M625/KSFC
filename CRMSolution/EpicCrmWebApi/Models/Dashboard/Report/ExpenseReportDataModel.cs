using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class ExpenseReportDataModel
    {
        public long EmployeeId { get; set; }
        public long DayId { get; set; }
        [Display(Name = "Employee Name")]
        public string Name { get; set; }
        [Display(Name = "Date")]
        public DateTime ExpenseDate { get; set; }

        public string ExpenseHQCode { get; set; }

        public EmployeeDailyConsolidationModel DailyConsolidation { get; set; }

        //[Display(Name = "Repair")]
        //public decimal RepairAmount { get; set; }
        //[Display(Name = "DA")]
        //public decimal DailyAllowanceAmount { get; set; }
        [Display(Name = "DA (Local)")]
        public decimal DALocalAmount { get; set; }
        [Display(Name = "DA (Outstation)")]
        public decimal DAOutstationAmount { get; set; }
        [Display(Name = "Telephone")]
        public decimal TelephoneAmount { get; set; }
        [Display(Name = "Internet")]
        public decimal InternetAmount { get; set; }
        [Display(Name = "Lodge Rent")]
        public decimal LodgeRent { get; set; }
        [Display(Name = "Veh.Repair")]
        public decimal VehicleRepairAmount { get; set; }
        [Display(Name = "Own Stay")]
        public decimal OwnStayAmount { get; set; }
        [Display(Name = "Toll Tax")]
        public decimal TollTaxAmount { get; set; }

        [Display(Name = "Driver Salary")]
        public decimal DriverSalary { get; set; }

        [Display(Name = "Stationary")]
        public decimal StationaryAmount { get; set; }

        [Display(Name = "Parking")]
        public decimal ParkingAmount { get; set; }
        [Display(Name = "Misc.")]
        public decimal MiscellaneousAmount { get; set; }
        public IEnumerable<TravelPublicExpenseDataModel> TravelPublic { get; set; }
        public IEnumerable<TravelPrivateExpenseDataModel> TravelPrivate { get; set; }
        public IEnumerable<TravelCompanyExpenseDataModel> TravelCompany { get; set; }
        public IEnumerable<FuelExpenseDataModel> Fuel { get; set; }

        [Display(Name = "Odometer(Self)")]
        public string GetTravelPrivateOdometerReadings
        {
            get
            {
                if (TravelPrivate == null || TravelPrivate.Count() == 0)
                {
                    return "";
                }
                else
                {
                    return String.Join(",", TravelPrivate.Select(x => $"{x.OdometerStart}-{x.OdometerEnd}").ToArray());
                }
            }
        }

        [Display(Name = "Odometer(Off)")]
        public string GetTravelCompanyOdometerReadings
        {
            get
            {
                if (TravelCompany == null || TravelCompany.Count() == 0)
                {
                    return "";
                }
                else
                {
                    return String.Join(",", TravelCompany.Select(x => $"{x.OdometerStart}-{x.OdometerEnd}").ToArray());
                }
            }
        }

        public long TravelPrivateDistanceInKm
        {
            get
            {
                if (TravelPrivate == null || TravelPrivate.Count() == 0)
                {
                    return 0;
                }
                else
                {
                    return TravelPrivate.Sum(x => x.OdometerEnd - x.OdometerStart);
                }
            }
        }

        public decimal CalculatedTrackingDistanceInKm
        {
            get
            {
                return decimal.Round(DailyConsolidation.TrackingDistanceInMeters / 1000, 1);
            }
        }

        public decimal DistanceDifference
        {
            get
            {
                return TravelPrivateDistanceInKm - CalculatedTrackingDistanceInKm;
            }
        }

        public decimal DistanceDifferenceInPercentage
        {
            get
            {
                decimal denominator = CalculatedTrackingDistanceInKm;
                if (denominator > 0)
                {
                    return Decimal.Round(DistanceDifference / CalculatedTrackingDistanceInKm * 100, 2);
                }

                return 0;
            }
        }

    }

    public class TravelPublicExpenseDataModel
    {
        [Display(Name = "Travel Pub.-VT")]
        public string TransportType { get; set; }
        [Display(Name = "Travel Pub.Amt")]
        public decimal Amount { get; set; }
    }

    public class TravelPrivateExpenseDataModel
    {
        public string TransportType { get; set; }
        [Display(Name = "Start Odo_Own")]
        public long OdometerStart { get; set; }
        [Display(Name = "End Odo_Own")]
        public long OdometerEnd { get; set; }
        [Display(Name = "Travel Pvt. Amt")]
        public decimal Amount { get; set; }
    }

    public class TravelCompanyExpenseDataModel
    {
        [Display(Name = "Start Odo_Com")]
        public long OdometerStart { get; set; }
        [Display(Name = "End Odo_Com")]
        public long OdometerEnd { get; set; }
    }

    public class FuelExpenseDataModel
    {
        [Display(Name = "Fuel Amt")]
        public decimal Amount { get; set; }

        public string FuelType { get; set; }
        [Display(Name = "Fuel In Ltr")]
        public decimal FuelQuantityInLiters { get; set; }
    }
}