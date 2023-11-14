using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DomainEntities;
using BusinessLayer;
using CRMUtilities;

namespace EpicCrmWebApi
{
    public class BonusPayment
    {
        public long AgreementId { get; set; }
        public IEnumerable<BonusCalculation> BonusRec { get; set; }
    }

    public class BonusMakePaymentModel : IValidatableObject
    {
        public IEnumerable<BonusPayment> BonusPayments { get; set; }
        [Display(Name = "Notes")]
        [MaxLength(100, ErrorMessage = "Notes can be maximum 100 characters.")]
        public string Comments { get; set; }
        public decimal TotalBonusAmount { get; set; }
        [MaxLength(50, ErrorMessage = "Payment Reference can be maximum 50 characters.")]
        public string PaymentReference { get; set; }

        public long RemitterAccountId { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Sender Info can be maximum 50 characters.")]
        public string SenderInfo { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Payment Type can be maximum 50 characters.")]
        public string PaymentType { get; set; }
        public string AgreementNumbers
        {
            get
            {
                return String.Join(" | ", BonusPayments.Select(x => x.BonusRec.FirstOrDefault().AgreementNumber));
            }
        }
        public DashboardBankAccount RemitterBankAccount { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(PaymentReference))
            {
                string error = $"{nameof(BonusMakePaymentModel)}: Payment reference is not specified.";
                Business.LogError(nameof(BonusMakePaymentModel), error);
                yield return new ValidationResult(error);
                yield break;
            }
            PaymentReference = Utils.TruncateString(PaymentReference, 50);

            Comments = Utils.TruncateString(Comments, 100);

            if (BonusPayments == null || BonusPayments.Count() == 0)
            {
                string error = $"{nameof(BonusMakePaymentModel)}: There are no Bonus Agreement sent for payment.";
                Business.LogError(nameof(BonusMakePaymentModel), error);
                yield return new ValidationResult(error);
                yield break;
            }

            decimal totalBonusAmount = 0;
            foreach (BonusPayment br in BonusPayments)
            {
                br.BonusRec = Business.GetSingleBonusDetails(br.AgreementId);

                if (br.BonusRec == null)
                {
                    string error = $"{nameof(STRMakePaymentModel)}: Bonus Agreement with AgreementId {br.AgreementId} | {br.BonusRec?.FirstOrDefault().AgreementNumber ?? ""} either does not exist or has changed recently.";
                    Business.LogError(nameof(STRMakePaymentModel), error);
                    yield return new ValidationResult(error);
                    yield break;
                }

                totalBonusAmount += (decimal)br.BonusRec.FirstOrDefault().BonusAmountPaid;
            }
            if (totalBonusAmount != TotalBonusAmount)
            {
                string error = $"{nameof(STRMakePaymentModel)}: Total bonus amount has changed from {TotalBonusAmount} to {totalBonusAmount}.";
                Business.LogError(nameof(STRMakePaymentModel), error);
                yield return new ValidationResult(error);
                yield break;
            }
            RemitterBankAccount = Business.GetBankAccountDetails(RemitterAccountId);
            if (RemitterBankAccount == null)
            {
                string error = $"{nameof(STRMakePaymentModel)}: Please select Remitter's account.";
                Business.LogError(nameof(STRMakePaymentModel), error);
                yield return new ValidationResult(error);
                yield break;
            }

            if (RemitterBankAccount == null || !RemitterBankAccount.IsActive)
            {
                string error = $"{nameof(STRMakePaymentModel)}: Remitter account {RemitterBankAccount.AccountNumber} is in-active.";
                Business.LogError(nameof(STRMakePaymentModel), error);
                yield return new ValidationResult(error);
                yield break;
            }
            var prfRec = Business.GetBonusPaymentReference(PaymentReference);
            if (prfRec != null)
            {
                string error = $"{nameof(STRMakePaymentModel)}: Payment Reference {PaymentReference} already exist. Changes are not saved.";
                Business.LogError(nameof(STRMakePaymentModel), error);
                yield return new ValidationResult(error);
                yield break;
            }
            yield break;

        }
    }
}