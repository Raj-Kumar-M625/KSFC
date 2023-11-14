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
    public class StockInputModel : IValidatableObject
    {
        public long Id { get; set; }
        public long StockInputTagId { get; set; }

        public string GRNNumber { get; set; }

        [Required]
        [Display(Name = "Line #")]
        [Range(1, 999, ErrorMessage = "Invalid Line #")]
        public int LineNumber { get; set; }

        public long ItemMasterId { get; set; }

        [Required]
        [Range(1, 99999, ErrorMessage = "Invalid Quantity")]
        public int Quantity { get; set; }

        [Required]
        [Range(1, 99999.99, ErrorMessage = "Invalid Rate")]
        public decimal Rate { get; set; }

        [Range(1, 9999999.99, ErrorMessage = "Invalid Total Bill Amount")]
        public decimal Amount { get; set; }

        [Display(Name = "Item")]
        public string ItemCode { get; set; }

        [Display(Name = "Item")]
        public string ItemDesc { get; set; }

        [Display(Name = "Item Type")]
        public string Category { get; set; }

        [Display(Name = "UOM")]
        public string Unit { get; set; }

        public long CyclicCount { get; set; }
        public long ParentCyclicCount { get; set; }
        public bool IsEditAllowed { get; set; }

        [Display(Name = "Delete this line item?")]
        public bool DeleteMe { get; set; } = false;

        public StockInputTag TagRec { get; set; }
        public StockInput Rec { get; set; } = null;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ItemMasterId <= 0)
            {
                yield return new ValidationResult("Please select an item.");
                yield break;
            }

            TagRec = Helper.GetSingleStockInputTag(StockInputTagId);

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
                Rec = Helper.GetSingleStockInputItem(StockInputTagId, Id);
            }
            else
            {
                Rec = new StockInput()
                {
                    Id = 0,
                    StockInputTagId = StockInputTagId,
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