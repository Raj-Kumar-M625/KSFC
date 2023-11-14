using BusinessLayer;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class GstRateViewModel : IValidatableObject
    {
        public long Id { get; set; }

        [Required]
        [Display(Name = "GST Code")]
        [MaxLength(20, ErrorMessage = "GST Code be maximum 20 characters")]
        public string GstCode { get; set; }

        [Required]
        [Display(Name = "GST Rate")]
        [Range(0.01, 100, ErrorMessage = "Please enter valid GST Rate")]
        [RegularExpression(@"((\d+)((\.\d{1,2})?))$", ErrorMessage = "Please enter valid GST Rate")]
        public decimal GstRate { get; set; }

        public bool IsRateEditable { get; set; }

        //[Required]
        [Display(Name = "Start Date")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true) ]
        //[CurrentDate(ErrorMessage = "Date must be after or equal to today's date")]
        public DateTime EffectiveStartDate { get; set; }

        [Display(Name = "Start Date")]
        [Required(ErrorMessage = "Start Date is required.")]
        public string EffectiveStartDateAsText { get; set; }

        public bool IsStartDateEditable { get; set; }

        [Display(Name = "End Date")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EffectiveEndDate { get; set; }

        [Required(ErrorMessage = "End Date is required.")]
        [Display(Name = "End Date")]
        public string EffectiveEndDateAsText { get; set; }

        public bool IsEndDateEditable { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            EffectiveStartDate = ConvertStringToDateTime(EffectiveStartDateAsText);
            EffectiveEndDate = ConvertStringToDateTime(EffectiveEndDateAsText);

            if (EffectiveStartDate == DateTime.MinValue)
            {
                yield return new ValidationResult("Please specify Start date.");
            }

            if (EffectiveEndDate == DateTime.MinValue)
            {
                yield return new ValidationResult("Please specify End date.");
            }

            EffectiveStartDate = EffectiveStartDate.Date;
            EffectiveEndDate = EffectiveEndDate.Date;

            DateTime today = Helper.GetCurrentIstDateTime().Date;
            if (EffectiveStartDate < today && IsStartDateEditable)
            {
                yield return new ValidationResult("Start Date can not be in the past.");
            }

            if (EffectiveEndDate < today && IsEndDateEditable)
            {
                yield return new ValidationResult("End Date can not be in the past.");
            }

            if (EffectiveEndDate < EffectiveStartDate)
            {
                yield return new ValidationResult("End Date must be greater than or equal to Start Date");
            }

            ICollection<DashboardGstRate> overlaps = Business.GetOverlappingGSTRates(GstCode, EffectiveStartDate, EffectiveEndDate);
            if (overlaps.Any(x => x.Id != Id))
            {
                yield return new ValidationResult("Date Range specified, conflicts with existing entries.");
            }
        }

        private DateTime ConvertStringToDateTime(string datetimeAsText)
        {
            var culture = CultureInfo.CreateSpecificCulture("en-GB");
            DateTime fDate = DateTime.MinValue;
            bool isValidDate = DateTime.TryParse(datetimeAsText, culture, DateTimeStyles.None, out fDate);
            return fDate;
        }
    }
}