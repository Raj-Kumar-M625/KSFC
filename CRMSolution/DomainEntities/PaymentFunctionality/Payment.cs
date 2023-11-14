using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class Payment : ApprovalData
    {
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public long DayId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string PaymentType { get; set; }
        public System.DateTime PaymentDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Comment { get; set; }
        public int ImageCount { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateUpdated { get; set; }
        public long SqlitePaymentId { get; set; }

        public string HQCode { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public bool IsActiveInSap { get; set; }
    }
}
