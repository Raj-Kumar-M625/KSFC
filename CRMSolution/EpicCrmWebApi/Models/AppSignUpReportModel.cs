using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class AppSignUpReportModel
    {
        public IEnumerable<AppSignUpReportDataModel> AppSignUpReportData { get; set; }
        public IEnumerable<OfficeHierarchyModel> OfficeHierarchy { get; set; }
    }
}