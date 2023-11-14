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
    public class STRPayment
    {
        public long STRTagId { get; set; }
        public VendorSTR strRec { get; set; }
    }

    public class STRMakePaymentModel : IValidatableObject
    {
        public IEnumerable<STRPayment> STRPayments { get; set; }

        [Display(Name = "Notes")]
        [MaxLength(100, ErrorMessage = "Notes can be maximum 100 characters.")]
        public string Comments { get; set; }

        public decimal TotalNetAmount { get; set; }

        [MaxLength(50, ErrorMessage = "Payment Reference can be maximum 50 characters.")]
        public string PaymentReference { get; set; }

        public long RemitterAccountId { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Sender Info can be maximum 50 characters.")]
        public string SenderInfo { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Payment Type can be maximum 50 characters.")]
        public string PaymentType { get; set; }

        public string STRNumbers
        {
            get
            {
                return String.Join("|", STRPayments.Select(x => x.strRec.STRNumber));
            }
        }

        public DashboardBankAccount RemitterBankAccount { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(PaymentReference))
            {
                string error = $"{nameof(STRMakePaymentModel)}: Payment reference is not specified.";
                Business.LogError(nameof(STRMakePaymentModel), error);
                yield return new ValidationResult(error);
                yield break;
            }
            PaymentReference = Utils.TruncateString(PaymentReference, 50);

            Comments = Utils.TruncateString(Comments, 100);

            if (STRPayments == null || STRPayments.Count() == 0)
            {
                string error = $"{nameof(STRMakePaymentModel)}: There are no STR sent for payment.";
                Business.LogError(nameof(STRMakePaymentModel), error);
                yield return new ValidationResult(error);
                yield break;
            }

            decimal totalNetAmount = 0;
            foreach (STRPayment sp in STRPayments)
            {
                sp.strRec = Business.GetSingleSTRForPayment(sp.STRTagId);

                if(sp.strRec == null)
                {
                    string error = $"{nameof(STRMakePaymentModel)}: STR with STRTagId {sp.STRTagId} | {sp.strRec?.STRNumber ?? ""} either does not exist or has changed recently.";
                    Business.LogError(nameof(STRMakePaymentModel), error);
                    yield return new ValidationResult(error);
                    yield break;
                }

                totalNetAmount += (decimal)sp.strRec.NetPayableAmount;
            }

            if (totalNetAmount != TotalNetAmount)
            {
                string error = $"{nameof(STRMakePaymentModel)}: Total net amount has changed from {TotalNetAmount} to {totalNetAmount}.";
                Business.LogError(nameof(STRMakePaymentModel), error);
                yield return new ValidationResult(error);
                yield break;
            }

            var prfRec = Business.GetSTRPaymentReference(PaymentReference);
            if (prfRec != null)
            {
                string error = $"{nameof(STRMakePaymentModel)}: Payment Reference {PaymentReference} already exist. Changes are not saved.";
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

            yield break;



        }


    }
}