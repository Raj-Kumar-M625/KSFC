using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class Order : ApprovalData
    {
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public long DayId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string OrderType { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalGST { get; set; }
        public decimal NetAmount { get; set; }


        public decimal RevisedTotalAmount { get; set; }
        public decimal RevisedTotalGST { get; set; }
        public decimal RevisedNetAmount { get; set; }


        public long ItemCount { get; set; }
        public DateTime DateUpdated { get; set; }

        public string HQCode { get; set; }

        public string Phone { get; set; }
        public string CustomerPhone { get; set; }

        public bool IsActive { get; set; }
        public bool IsActiveInSap { get; set; }

        public string DiscountType { get; set; }
        public int ImageCount { get; set; }
    }
}
