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
    public class StockFulfillItem
    {
        public long StockBalanceId { get; set; }
        public int IssueQty { get; set; }
        public long CyclicCount { get; set; }

        public StockBalance StockBalanceRec { get; set; }
    }

    public class StockFulfillModel : IValidatableObject
    {
        public long StockRequestId { get; set; }
        public long ItemMasterid { get; set; }
        public long CyclicCount { get; set; }

        [MaxLength(100, ErrorMessage = "Notes field can be maximum 100 characters.")]
        public string Notes { get; set; }

        public IEnumerable<StockFulfillItem> FulfillItems { get; set; }
        public StockRequestFull StockRequestFullRec { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FulfillItems == null || FulfillItems.Count() == 0)
            {
                string error = $"{nameof(StockFulfillModel)}: There are no fulfillment items.";
                Business.LogError(nameof(StockFulfillModel), error);
                yield return new ValidationResult(error);
                yield break;
            }

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

            // retrieve original records
            long totalIssueQty = 0;
            foreach(StockFulfillItem dp in FulfillItems)
            {
                DomainEntities.StockLedgerFilter s = Helper.GetDefaultStockLedgerFilter();
                s.ApplyRecordIdFilter = true;
                s.RecordId = dp.StockBalanceId;
                dp.StockBalanceRec = Business.GetStockBalance(s).FirstOrDefault();

                if (dp.StockBalanceRec == null || dp.StockBalanceRec.CyclicCount != dp.CyclicCount)
                {
                    string error = $"{nameof(StockFulfillModel)}: Stock Balance  with Id {dp.StockBalanceId} either does not exist or has changed recently. Please refresh the page and try again.";
                    Business.LogError(nameof(StockFulfillModel), error);
                    yield return new ValidationResult(error);
                    yield break;
                }

                if (dp.IssueQty == 0 || dp.IssueQty > dp.StockBalanceRec.StockQuantity)
                {
                    string error = $"{nameof(StockFulfillModel)}: Invalid Issue Qty. {dp.IssueQty} specified. The value is either 0 or greater than available stock balance.";
                    Business.LogError(nameof(StockFulfillModel), error);
                    yield return new ValidationResult(error);
                    yield break;
                }

                dp.StockBalanceRec.IssueQuantity = dp.IssueQty;

                totalIssueQty += dp.IssueQty;
            }

            if (totalIssueQty == 0 || totalIssueQty > StockRequestFullRec.Quantity)
            {
                string error = $"{nameof(StockFulfillModel)}: Total Issue Qty. {totalIssueQty} is Invalid. Expected value between 1 and {StockRequestFullRec.Quantity}";
                Business.LogError(nameof(StockFulfillModel), error);
                yield return new ValidationResult(error);
                yield break;
            }

            yield break;
        }
    }
}