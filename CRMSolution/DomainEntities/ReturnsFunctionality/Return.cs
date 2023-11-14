using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class Return : ApprovalData
    {
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public long DayId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal TotalAmount { get; set; }
        public long ItemCount { get; set; }
        public string ReferenceNumber { get; set; }
        public string Comments { get; set; }
        public string HQCode { get; set; }

        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public bool IsActiveInSap { get; set; }
    }
}
