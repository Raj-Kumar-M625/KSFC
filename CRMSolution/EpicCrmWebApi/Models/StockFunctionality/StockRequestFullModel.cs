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
    public class StockRequestFullModel : StockTagModel, IValidatableObject
    {
        public long Id { get; set; }
        public long StockRequestTagId { get; set; }
        public long ItemMasterId { get; set; }

        [Display(Name = "Req. #")]
        public string RequestNumber { get; set; }

        [Display(Name = "Request Date")]
        public System.DateTime RequestDate { get; set; }

        [Display(Name = "Notes (100 chars)")]
        [MaxLength(100, ErrorMessage = "Notes can be maximum 100 characters.")]
        public string Notes { get; set; }

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

        //public StockRequestTag TagRec { get; set; }
        //public StockRequest Rec { get; set; } = null;

        public long CyclicCount { get; set; }
        public bool IsPendingFulfillment { get; set; }

        [Display(Name = "Status Quantity")]
        public int QuantityIssued { get; set; }
        [Display(Name = "Status Date")]
        public Nullable<System.DateTime> IssueDate { get; set; }
        public long StockLedgerId { get; set; }

        public string ReviewNotes { get; set; }

        public string RequestType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield break;
            //if (ItemMasterId <= 0)
            //{
            //    yield return new ValidationResult("Please select an item.");
            //    yield break;
            //}

            //TagRec = Helper.GetSingleStockRequestTag(StockRequestTagId);

            //if ((TagRec?.IsEditAllowed ?? false) == false)
            //{
            //    yield return new ValidationResult("The requested action (1) is not available. Please refresh the page and try again.");
            //    yield break;
            //}

            //if (TagRec.CyclicCount != ParentCyclicCount)
            //{
            //    yield return new ValidationResult("The requested action (2) is not available. Please refresh the page and try again.");
            //    yield break;
            //}

            //// retrieve original record
            //if (Id > 0)
            //{
            //    Rec = Helper.GetSingleStockRequestItem(StockRequestTagId, Id);
            //}
            //else
            //{
            //    Rec = new StockRequest()
            //    {
            //        Id = 0,
            //        StockRequestTagId = StockRequestTagId,
            //        Status = StockStatus.PendingFulfillment,
            //        CyclicCount = 1
            //    };
            //}

            //if (Rec == null)
            //{
            //    yield return new ValidationResult("The requested action (3) is not available. Please refresh the page and try again.");
            //    yield break;
            //}
            //else
            //{
            //    Rec.ParentCyclicCount = ParentCyclicCount;
            //}

            //if (Id <= 0 && DeleteMe)
            //{
            //    yield return new ValidationResult("The requested action (4) is not available. Please refresh the page and try again.");
            //    yield break;
            //}
        }
    }
}