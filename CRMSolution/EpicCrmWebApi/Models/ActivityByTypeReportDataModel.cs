using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class ActivityByTypeReportDataModel
    {
        public IEnumerable<ActivityByTypeReportModel> ActivityByTypeReport { get; set; }
    }
}