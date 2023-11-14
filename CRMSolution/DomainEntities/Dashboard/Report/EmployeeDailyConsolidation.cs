using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class EmployeeDailyConsolidation : TrackingData
    {
        public long EmployeeId { get; set; }
        public long DayId { get; set; }
        public string StaffCode { get; set; }
        public decimal TotalOrderAmount { get; set; }
        public decimal TotalReturnOrderAmount { get; set; }
        public decimal TotalExpenseAmount { get; set; }
        public decimal TotalPaymentAmount { get; set; }
        public long ActivityCount { get; set; }
    }
}
