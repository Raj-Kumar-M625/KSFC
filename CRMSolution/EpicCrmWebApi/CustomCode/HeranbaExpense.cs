using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class HeranbaExpense : IExpenseCalc
    {
        public void FillCalculatedData(ExpenseReportData erd, EmployeeExpenseReportDataModel eerdm)
        {
            eerdm.ModeAndClassOfFare = ModeAndClassOfFare(erd);
            eerdm.LodgeRent = LodgeRent(erd);
            eerdm.LocalConveyance = LocalConveyance(erd);
            eerdm.OutstationConveyance = 0;
            eerdm.IncdlCharges = IncdlCharges(erd);
            eerdm.CommunicationExpenses = CommunicationExpenses(erd);
        }

        private static Decimal ModeAndClassOfFare(ExpenseReportData x)
        {
            return x.Fuel.Sum(a => a.Amount) + x.ParkingAmount + x.TollTaxAmount
                                               + x.TravelPrivate.Sum(a => a.Amount)
                                               + x.TravelPublic.Sum(a => a.Amount)
                                               + x.VehicleRepairAmount;
        }

        private static Decimal LodgeRent(ExpenseReportData erd) => erd.OwnStayAmount + erd.LodgeRent + erd.DAOutstationAmount;

        private static Decimal LocalConveyance(ExpenseReportData erd) => erd.DALocalAmount;

        private static Decimal IncdlCharges(ExpenseReportData erd) => erd.InternetAmount + erd.MiscellaneousAmount;

        private static Decimal CommunicationExpenses(ExpenseReportData erd) => erd.TelephoneAmount;
    }
}