using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteDayPlanTarget
    {
        public string Id { get; set; }
        public DateTime DayPlanningTimeStamp { get; set; }
        public decimal TargetSales { get; set; }
        public decimal TargetCollection { get; set; }
        public int TargetNewDealerAppointment { get; set; }
        public int TargetDemoActivity { get; set; }
        public int TargetVigoreSales { get; set; }
    }
}