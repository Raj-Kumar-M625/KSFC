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
    public class StockFulfillDenyModel : IValidatableObject
    {
        public long StockRequestId { get; set; }
        public long ItemMasterid { get; set; }
        public long CyclicCount { get; set; }

        [MaxLength(100, ErrorMessage = "Notes field can be maximum 100 characters.")]
        public string Notes { get; set; }

        public StockRequestFull StockRequestFullRec { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // retrieve original record
            DomainEntities.StockRequestFilter criteria = Helper.GetDefaultStockRequestFilter();
            criteria.ApplyStockRequestIdFilter = true;
            criteria.StockRequestId = StockRequestId;
            criteria.TagRecordStatus = StockStatus.RequestApproved.ToString();
            StockRequestFullRec = Business.GetStockRequestItems(criteria)?.FirstOrDefault();
            if (StockRequestFullRec == null)
            {
                yield return new ValidationResult("The record does not exist. Please refresh the page and try again.");
                yield break;
            }

            // cyclic count check
            if (StockRequestFullRec.CyclicCount != CyclicCount || StockRequestFullRec.ItemMasterId != ItemMasterid)
            {
                yield return new ValidationResult("The record has changed recently in the background. Please refresh the page and try again.");
                yield break;
            }

            yield break;
        }
    }
}