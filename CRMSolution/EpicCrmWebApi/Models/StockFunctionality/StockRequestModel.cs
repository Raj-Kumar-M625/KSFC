using BusinessLayer;
using CRMUtilities;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class StockRequestModel : IValidatableObject
    {
        public long Id { get; set; }
        public long StockRequestTagId { get; set; }

        public string RequestNumber { get; set; }

        public long ItemMasterId { get; set; }

        [Required]
        [Range(1, 99999, ErrorMessage = "Invalid Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Item")]
        public string ItemCode { get; set; }

        [Display(Name = "Item")]
        public string ItemDesc { get; set; }

        [Display(Name = "Item Type")]
        public string Category { get; set; }

        [Display(Name = "UOM")]
        public string Unit { get; set; }

        public string Status { get; set; }

        public long CyclicCount { get; set; }
        public long ParentCyclicCount { get; set; }
        public bool IsEditAllowed { get; set; }

        [Display(Name = "Delete this line item?")]
        public bool DeleteMe { get; set; } = false;

        [Display(Name = "Status Quantity")]
        public int QuantityIssued { get; set; }
        [Display(Name = "Status Date")]
        public Nullable<System.DateTime> IssueDate { get; set; }
        public long StockLedgerId { get; set; }
        [Display(Name = "Review Notes")]
        public string ReviewNotes { get; set; }

        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }

        [Display(Name = "Date Updated")]
        public DateTime DateUpdated { get; set; }

        public StockRequestTag TagRec { get; set; }
        public StockRequest Rec { get; set; } = null;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ItemMasterId <= 0)
            {
                yield return new ValidationResult("Please select an item.");
                yield break;
            }

            TagRec = Helper.GetSingleStockRequestTag(StockRequestTagId);

            if ((TagRec?.IsEditAllowed ?? false) == false)
            {
                yield return new ValidationResult("The requested action (1) is not available. Please refresh the page and try again.");
                yield break;
            }

            if (TagRec.CyclicCount != ParentCyclicCount)
            {
                yield return new ValidationResult("The requested action (2) is not available. Please refresh the page and try again.");
                yield break;
            }

            // retrieve original record
            if (Id > 0)
            {
                Rec = Helper.GetSingleStockRequestItem(StockRequestTagId, Id);
            }
            else
            {
                Rec = new StockRequest()
                {
                    Id = 0,
                    StockRequestTagId = StockRequestTagId,
                    Status = StockStatus.Pending.ToString(),
                    CyclicCount = 1
                };
            }

            if (Rec == null)
            {
                yield return new ValidationResult("The requested action (3) is not available. Please refresh the page and try again.");
                yield break;
            }
            else
            {
                Rec.ParentCyclicCount = ParentCyclicCount;
            }

            if (Id <= 0 && DeleteMe)
            {
                yield return new ValidationResult("The requested action (4) is not available. Please refresh the page and try again.");
                yield break;
            }
        }
    }
}