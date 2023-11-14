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
    public class DWSPayment
    {
        public long Id { get; set; }
        public long CyclicCount { get; set; }
        public DWS origDWSRec { get; set; }
    }

    public class DWSMakePaymentModel : IValidatableObject
    {
        public IEnumerable<DWSPayment> DWSPayments { get; set; }

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

        public string DWSNumbers
        {
            get
            {
                return String.Join("|", DWSPayments.Select(x => x.origDWSRec.DWSNumber));
            }
        }

        public DashboardBankAccount RemitterBankAccount { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(PaymentReference))
            {
                string error = $"{nameof(DWSMakePaymentModel)}: Payment reference is not specified.";
                Business.LogError(nameof(DWSMakePaymentModel), error);
                yield return new ValidationResult(error);
                yield break;
            }

            PaymentReference = Utils.TruncateString(PaymentReference, 50);

            Comments = Utils.TruncateString(Comments, 100);


            if (DWSPayments == null || DWSPayments.Count() == 0)
            {
                string error = $"{nameof(DWSMakePaymentModel)}: There are no DWS sent for payment.";
                Business.LogError(nameof(DWSMakePaymentModel), error);
                yield return new ValidationResult(error);
                yield break;
            }


            // retrieve original records
            decimal totalNetAmount = 0;
            foreach(DWSPayment dp in DWSPayments)
            {
                dp.origDWSRec = Business.GetSingleDWS(dp.Id);

                if (dp.origDWSRec == null || dp.origDWSRec.CyclicCount != dp.CyclicCount)
                {
                    string error = $"{nameof(DWSMakePaymentModel)}: DWS with Id {dp.Id} | {dp.origDWSRec?.DWSNumber ?? ""} either does not exist or has changed recently.";
                    Business.LogError(nameof(DWSMakePaymentModel), error);
                    yield return new ValidationResult(error);
                    yield break;
                }

                totalNetAmount += dp.origDWSRec.NetPayable;
            }

            if (totalNetAmount != TotalNetAmount)
            {
                string error = $"{nameof(DWSMakePaymentModel)}: Total net amount has changed from {TotalNetAmount} to {totalNetAmount}.";
                Business.LogError(nameof(DWSMakePaymentModel), error);
                yield return new ValidationResult(error);
                yield break;
            }

            // ensure payment reference is not duplicate
            var prfRec = Business.GetPaymentReference(PaymentReference);
            if (prfRec != null)
            {
                string error = $"{nameof(DWSMakePaymentModel)}: Payment Reference {PaymentReference} already exist. Changes are not saved.";
                Business.LogError(nameof(DWSMakePaymentModel), error);
                yield return new ValidationResult(error);
                yield break;
            }

            // validate remitter Account
            RemitterBankAccount = Business.GetBankAccountDetails(RemitterAccountId);
            if (RemitterBankAccount == null)
            {
                string error = $"{nameof(DWSMakePaymentModel)}: Please select Remitter's account.";
                Business.LogError(nameof(DWSMakePaymentModel), error);
                yield return new ValidationResult(error);
                yield break;
            }

            if (RemitterBankAccount == null || !RemitterBankAccount.IsActive)
            {
                string error = $"{nameof(DWSMakePaymentModel)}: Remitter account {RemitterBankAccount.AccountNumber} is in-active.";
                Business.LogError(nameof(DWSMakePaymentModel), error);
                yield return new ValidationResult(error);
                yield break;
            }

            yield break;
        }
    }
}