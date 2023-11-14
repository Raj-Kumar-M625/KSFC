using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class ExpenseReportModel
    {
        public IEnumerable<ExpenseReportDataModel> ExpenseReportData { get; set; }
        public IEnumerable<OfficeHierarchyModel> OfficeHierarchy { get; set; }
    }
}