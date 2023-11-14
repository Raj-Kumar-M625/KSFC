using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class PJMargoExpense : IExpenseCalc
    {
        public void FillCalculatedData(ExpenseReportData erd, EmployeeExpenseReportDataModel eerdm)
        {
            eerdm.ModeAndClassOfFare = ModeAndClassOfFare(erd);
            eerdm.LodgeRent = LodgeRent(erd);
            eerdm.LocalConveyance = LocalConveyance(erd);
            eerdm.OutstationConveyance = OutstationConveyance(erd);
            eerdm.IncdlCharges = IncdlCharges(erd);
            eerdm.CommunicationExpenses = CommunicationExpenses(erd);
        }

        public static Decimal ModeAndClassOfFare(ExpenseReportData x)
        {
            return x.Fuel.Sum(a => a.Amount) + x.ParkingAmount + x.TollTaxAmount
                                               + x.TravelPublic.Sum(a => a.Amount)
                                               + x.VehicleRepairAmount;
        }

        public static Decimal LodgeRent(ExpenseReportData erd) => erd.LodgeRent;

        public static Decimal LocalConveyance(ExpenseReportData erd)
                => erd.DALocalAmount + erd.DailyAllowanceAmount;

        public static Decimal OutstationConveyance(ExpenseReportData erd)
                => erd.DAOutstationAmount;

        public static Decimal IncdlCharges(ExpenseReportData erd) => erd.InternetAmount + erd.MiscellaneousAmount;

        public static Decimal CommunicationExpenses(ExpenseReportData erd) => erd.TelephoneAmount;
    }
}