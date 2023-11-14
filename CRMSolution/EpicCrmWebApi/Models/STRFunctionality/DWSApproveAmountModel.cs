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
    public class DWSApproveAmountModel : IValidatableObject
    {
        public long Id { get; set; }
        public long STRTagId { get; set; }

        [Display(Name = "Notes")]
        [MaxLength(500, ErrorMessage = "Notes can be maximum 500 characters.")]
        public string Comments { get; set; }

        public decimal DeductAmount { get; set; }

        public long CyclicCount { get; set; }
        public long StrTagCyclicCount { get; set; }

        public long BeneficiaryAccountId { get; set; }

        public DomainEntities.DWS OrigRec { get; set; }

        public EntityBankDetail BeneficiaryBankAccount { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            OrigRec = Business.GetSingleDWS(Id);

            if (OrigRec.CyclicCount != CyclicCount)
            {
                string error = $"{nameof(DWSApproveAmountModel)}: Cyclic count mismatch for DWS {Id} | {OrigRec.DWSNumber}";
                Business.LogError(nameof(DWSApproveAmountModel), error);
                yield return new ValidationResult(error);
                yield break;
            }

            if (DeductAmount < 0 || DeductAmount > OrigRec.GoodsPrice)
            {
                string error = $"{nameof(DWSApproveAmountModel)}: Invalid Deduct Amount {DeductAmount} for DWS {Id} | {OrigRec.DWSNumber}";
                Business.LogError(nameof(DWSApproveAmountModel), error);
                yield return new ValidationResult(error);
                yield break;
            }

            BeneficiaryBankAccount = Business.GetSingleBankDetail(BeneficiaryAccountId);

            if (BeneficiaryBankAccount == null)
            {
                string error = $"{nameof(DWSApproveAmountModel)}: Please select Beneficiary account.";
                Business.LogError(nameof(DWSApproveAmountModel), error);
                yield return new ValidationResult(error);
                yield break;
            }

            if (BeneficiaryBankAccount == null || !BeneficiaryBankAccount .IsActive || !BeneficiaryBankAccount.IsApproved)
            {
                string error = $"{nameof(DWSApproveAmountModel)}: Beneficiary account {BeneficiaryBankAccount.BankAccount} is not in approved/active state.";
                Business.LogError(nameof(DWSApproveAmountModel), error);
                yield return new ValidationResult(error);
                yield break;
            }

            yield break;
        }
    }
}