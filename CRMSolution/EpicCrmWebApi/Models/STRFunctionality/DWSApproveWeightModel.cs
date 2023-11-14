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
    public class DWSApproveWeightModel : IValidatableObject
    {
        public long Id { get; set; }
        public long STRTagId { get; set; }

        [Display(Name = "Notes")]
        [MaxLength(500, ErrorMessage = "Notes can be maximum 500 characters.")]
        public string Comments { get; set; }

        [Display(Name = "Silo Deduct Wt. Override")]
        public decimal SiloDeductWtOverride { get; set; }

        public long CyclicCount { get; set; }
        public long StrTagCyclicCount { get; set; }

        public DomainEntities.DWS OrigRec { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            OrigRec = Business.GetSingleDWS(Id);

            if (OrigRec.CyclicCount != CyclicCount)
            {
                string error = $"{nameof(DWSApproveWeightModel)}: Cyclic count mismatch for DWS {Id} | {OrigRec.DWSNumber}";
                Business.LogError(nameof(DWSApproveWeightModel), error);
                yield return new ValidationResult(error);
                yield break;
            }

            if (SiloDeductWtOverride < 0 || SiloDeductWtOverride > OrigRec.GoodsWeight)
            {
                string error = $"{nameof(DWSApproveWeightModel)}: Invalid Override Wt. {SiloDeductWtOverride} for DWS {Id} | {OrigRec.DWSNumber}";
                Business.LogError(nameof(DWSApproveWeightModel), error);
                yield return new ValidationResult(error);
                yield break;
            }

            yield break;
        }
    }
}