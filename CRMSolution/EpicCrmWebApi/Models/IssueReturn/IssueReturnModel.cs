using BusinessLayer;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class IssueReturnModel : IValidatableObject
    {
        public long Id { get; set; }
        public string ReportType { get; set; }
        public long EmployeeId { get; set; }

        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        public long DayId { get; set; }

        [Display(Name = "Date")]
        public System.DateTime TransactionDate { get; set; }

        public string TransactionDateAsText { get; set; }

        public string StaffCode { get; set; }

        public long EntityAgreementId { get; set; }

        [Display(Name = "Agreement")]
        public string AgreementNumber { get; set; }
        public long WorkflowSeasonId { get; set; }

        [Display(Name = "Season")]
        public string WorkflowSeasonName { get; set; }
        [Display(Name = "Crop")]
        public string TypeName { get; set; } // crop name goes here

        public long EntityId { get; set; }
        [Display(Name = "Entity Type")]
        public string EntityType { get; set; }
        [Display(Name = "Entity Name")]
        public string EntityName { get; set; }

        public long ItemMasterId { get; set; }
        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }
        [Display(Name = "Item Type")]
        public string ItemType { get; set; }
        [Display(Name = "UOM")]
        public string ItemUnit { get; set; }

        [Display(Name = "Activity Type")]
        public string TransactionType { get; set; }
        public int Quantity { get; set; }

        [Display(Name = "Employee Phone")]
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public bool IsActiveInSap { get; set; }

        public long ActivityId { get; set; }

        [Display(Name = "Slip No.")]
        public string SlipNumber { get; set; }

        [Display(Name = "Land Size (Acres)")]
        public decimal LandSizeInAcres { get; set; }

        [Display(Name = "Item Rate")]
        public decimal ItemRate { get; set; }

        [Display(Name = "Applied Activity Type")]
        public string AppliedTransactionType { get; set; }

        public long AppliedItemMasterId { get; set; }

        [Display(Name = "Applied Item Type")]
        public string AppliedItemType { get; set; }

        [Display(Name = "Applied Item Code")]
        public string AppliedItemCode { get; set; }

        [Display(Name = "Applied UOM")]
        public string AppliedItemUnit { get; set; }
        [Display(Name = "Applied Quantity")]
        public int AppliedQuantity { get; set; }
        [Display(Name = "Applied Item Rate")]
        public decimal AppliedItemRate { get; set; }

        [Required]
        public string Status { get; set; }

        public DateTime DateUpdated { get; set; }

        [MaxLength(100, ErrorMessage = "Comments can be maximum 100 characters.")]
        public string Comments { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        public long CyclicCount { get; set; }

        public bool IsIssueItem { get; set; }

        [Display(Name = "Zone")]
        [MaxLength(50, ErrorMessage = "Invalid Zone")]
        public string ZoneCode { get; set; }

        [Display(Name = "Area")]
        [MaxLength(50, ErrorMessage = "Invalid Area")]
        public string AreaCode { get; set; }

        [Display(Name = "Territory")]
        [MaxLength(50, ErrorMessage = "Invalid Territory")]
        public string TerritoryCode { get; set; }

        [Display(Name = "HQ")]
        [MaxLength(50, ErrorMessage = "Invalid HQ")]
        public string HQCode { get; set; }
        public IssueReturn OrigRec { get; set; }
        public ItemMaster SelectedItem { get; set; }
        public StockBalance StockBalanceRec { get; set; }

        public bool IsApproved => "Approved".Equals(Status, StringComparison.OrdinalIgnoreCase);
        public bool IsRejected => "Rejected".Equals(Status, StringComparison.OrdinalIgnoreCase);

        public bool IsPending => "Pending".Equals(Status, StringComparison.OrdinalIgnoreCase);
        public decimal ItemValue
        {
            get
            {
                if (IsPending)
                {
                    return (Quantity * ItemRate);
                }
                else if (IsApproved)
                {
                    return (AppliedQuantity * AppliedItemRate);
                }
                return 0;
            }
        }

        public bool IsIssue => AppliedTransactionType?.Contains("Issue") ?? false;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            TransactionDate = Helper.ConvertStringToDateTime(TransactionDateAsText);

            if (TransactionDate == DateTime.MinValue)
            {
                yield return new ValidationResult("Please specify Date.");
                yield break;
            }

            TransactionDate = TransactionDate.Date;

            DateTime today = Helper.GetCurrentIstDateTime().Date;
            if (TransactionDate > today)
            {
                yield return new ValidationResult("Date can not be in the future.");
                yield break;
            }

            if (AppliedItemMasterId <= 0)
            {
                yield return new ValidationResult("Please select an item.");
                yield break;
            }

            // retrieve original Record
            DomainEntities.SearchCriteria sc = Helper.GetDefaultSearchCriteria();
            sc.ApplyIdFilter = true;
            sc.FilterId = Id;

            ICollection<IssueReturn> issueReturns = Business.GetIssueReturns(sc);

            if ((issueReturns?.Count ?? 0) != 1)
            {
                yield return new ValidationResult("An error occured while retrieving original record. Please refresh the page and try again.");
                yield break;
            }

            OrigRec = issueReturns.First();

            // retrieve item master
            ICollection<DomainEntities.ItemMaster> itemsData = Business.GetAllItemMaster();
            SelectedItem = itemsData.Where(x => x.Id == AppliedItemMasterId &&
                x.Category.Equals(AppliedItemType, StringComparison.OrdinalIgnoreCase) &&
                x.TypeNames.Any(y => y.TypeName.Equals(OrigRec.TypeName, StringComparison.OrdinalIgnoreCase)))
                .FirstOrDefault();

            if (SelectedItem == null)
            {
                yield return new ValidationResult($"Please select a valid '{AppliedItemType}' Item.");
                yield break;
            }

            if (IsRejected && AppliedQuantity != 0)
            {
                yield return new ValidationResult($"Applied Quantity is invalid for the selected Status. It must be zero when status is not approved/rejected.");
                yield break;
            }

            // check stock balance
            DomainEntities.StockLedgerFilter slf = Helper.GetDefaultStockLedgerFilter();
            slf.ApplyItemIdFilter = true;
            slf.ItemId = SelectedItem.Id;
            slf.ApplyEmployeeCodeFilter = true;
            slf.EmployeeCode = OrigRec.EmployeeCode;

            if (IsApproved)
            {
                IEnumerable<StockBalance> balances = Business.GetStockBalance(slf);
                StockBalanceRec = balances.FirstOrDefault();
                if (IsIssue)
                {
                    if (StockBalanceRec == null ||
                        StockBalanceRec.StockQuantity < AppliedQuantity)
                    {
                        yield return new ValidationResult($"There are not enough items in the stock. Available {StockBalanceRec?.StockQuantity ?? 0} | Requested {AppliedQuantity}");
                        yield break;
                    }
                }
            }

            yield break;
        }
    }
}