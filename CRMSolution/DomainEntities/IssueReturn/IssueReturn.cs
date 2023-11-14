using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class IssueReturn
    {
        public long Id { get; set; }
        public string ReportType { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public long DayId { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public string StaffCode { get; set; }
        public string HQCode { get; set; }

        public long EntityAgreementId { get; set; }
        public string AgreementNumber { get; set; }
        public long WorkflowSeasonId { get; set; }
        public string WorkflowSeasonName { get; set; }
        public string TypeName { get; set; } // crop name goes here
        public long EntityId { get; set; }
        public string EntityType { get; set; }
        public string EntityName { get; set; }

        public long ItemMasterId { get; set; }
        public string ItemType { get; set; }
        public string ItemCode { get; set; }
        public string ItemUnit { get; set; }
        
        public string TransactionType { get; set; }
        public int Quantity { get; set; }

        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public bool IsActiveInSap { get; set; }

        public long ActivityId { get; set; }

        public string SlipNumber { get; set; }
        public decimal LandSizeInAcres { get; set; }
        public decimal ItemRate { get; set; }

        public string  AppliedTransactionType { get; set; }
        public long AppliedItemMasterId { get; set; }
        public string AppliedItemType { get; set; }
        public string AppliedItemCode { get; set; }
        public string AppliedItemUnit { get; set; }
        public int AppliedQuantity { get; set; }
        public decimal AppliedItemRate { get; set; }
        public decimal Rate { get; set; }
        public decimal ReturnRate { get; set; }
        public string Status { get; set; }

        public string Comments { get; set; }
        public string CreatedBy { get; set; }

        public DateTime DateUpdated { get; set; }

        public string CurrentUser { get; set; }


        public long CyclicCount { get; set; }
        public DateTime CurrentIstTime { get; set; }

        public bool IsIssueItem
        {
            get
            {
                if (String.IsNullOrEmpty(AppliedTransactionType))
                {
                    return false;
                }

                return AppliedTransactionType.Contains("Issue");
            }
        }

        public bool IsApproved => "Approved".Equals(Status, StringComparison.OrdinalIgnoreCase);

        public long StockBalanceRecId { get; set; }
    }
}
