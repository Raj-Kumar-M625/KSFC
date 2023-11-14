using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public interface IExpenseCalc
    {
        void FillCalculatedData(ExpenseReportData erd, EmployeeExpenseReportDataModel eerdm);
    }
}