using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class ExpenseItemViewModel : IValidatableObject
    {
        public long Id { get; set; }
        public long ExpenseId { get; set; }
        public int SequenceNumber { get; set; }

        [Display(Name = "Amount (Rs.)")]
        public decimal Amount { get; set; }

        [Display(Name = "Deducted Amount (Rs.)")]
        public decimal DeductedAmount { get; set; }

        [Display(Name = "Revised Amount (Rs.)")]
        public decimal RevisedAmount { get; set; }

        [Display(Name = "Expense Type")]
        public string ExpenseType { get; set; }
        //public string ExpenseTypeAsString => String.IsNullOrEmpty(TransportType) ? ExpenseType : TransportType;

        [Display(Name = "Odometer")]
        public long OdometerStart { get; set; }
        public long OdometerEnd { get; set; }
        [Display(Name = "Transport Type")]
        public string TransportType { get; set; }

        public string Odometer
        {
            get
            {
                if (OdometerStart > 0 && OdometerEnd > 0)
                {
                    return $"{OdometerStart}-{OdometerEnd}";
                }

                if (OdometerStart > 0)
                {
                    return $"{OdometerStart}";
                }

                if (OdometerEnd > 0)
                {
                    return $"{OdometerStart}-{OdometerEnd}";
                }

                return "";
            }
        }

        [Display(Name = "Fuel Type")]
        public string FuelType { get; set; }
        [Display(Name = "Fuel Qty.(ltr)")]
        public Decimal FuelQuantityInLiters { get; set; }

        [Display(Name = "Notes")]
        public string Comment { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DeductedAmount > Amount)
            {
                yield return new ValidationResult("DeductedAmount can't be greater than Amount.");
                yield break;
            }
        }
    }
}