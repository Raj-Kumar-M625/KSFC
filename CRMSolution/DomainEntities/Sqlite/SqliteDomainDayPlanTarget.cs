using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SqliteDomainDayPlanTarget
    {
        public string PhoneDbId { get; set; }
        public DateTime PlanTimeStamp { get; set; }
        public decimal TargetSales { get; set; }
        public decimal TargetCollection { get; set; }
        public int TargetNewDealerAppointment { get; set; }
        public int TargetDemoActivity { get; set; }
        public decimal TargetVigoreSales { get; set; }
    }

    public class SqliteDayPlanTargetData : SqliteDomainDayPlanTarget
    {
        public long Id { get; set; }
        public long BatchId { get; set; }
        public long EmployeeId { get; set; }
        public bool IsProcessed { get; set; }
        public long DayPlanTargetId { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateUpdated { get; set; }
    }
}