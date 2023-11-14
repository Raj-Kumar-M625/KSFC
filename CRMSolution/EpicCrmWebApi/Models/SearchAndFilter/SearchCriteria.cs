using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SearchCriteria : IValidatableObject
    {
        public string Zone { get; set; }
        public string Area { get; set; }
        public string Territory { get; set; }
        public string HQ { get; set; }
        public string DataStatus { get; set; }
        public string ReportType { get; set; }
        public string AmountFrom { get; set;}
        public string AmountTo { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }

        public string PlannedDateFrom { get; set; }
        public string PlannedDateTo { get; set; }
        public string HarvestDate { get; set; }

        public string ActivityType { get; set; }
        public string ClientType { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public int EmployeeStatus { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string ClientName { get; set; }
        public int WorkFlowStatus { get; set; }
        public string WorkFlow { get; set; }
        public string EntityName { get; set; }
        public long Id { get; set; }
        public string Distance { get; set; }
        public string AgreementNumber { get; set; }
        public string SlipNumber { get; set; }
        public string RowStatus { get; set; }

        public string AgreementStatus { get; set; }
        public string Crop { get; set; }

        //PK
        public string TargetStatus { get; set; }
        public string DayPlanType { get; set; }

        //Rajesh V
        public string UniqueId { get; set; }
        public string SeasonName { get; set; }
        //Author: Venkatesh
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public int GeoTagStatus { get; set; }
        public int ProfileStatus { get; set; }
        public string BankDetailStatus { get; set; }
        public string BusinessRole { get; set; }

        public DateTime[] searchDates = new DateTime[5];
        public bool[] dateParseStatus = new bool[5];

        public long QuestionPaperId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (String.IsNullOrEmpty(DateFrom))
            {
                DateFrom = "01-01-0001";
            }

            if (String.IsNullOrEmpty(DateTo))
            {
                DateTo = "01-01-0001";
            }

            if (String.IsNullOrEmpty(PlannedDateFrom))
            {
                PlannedDateFrom = "01-01-0001";
            }

            if (String.IsNullOrEmpty(PlannedDateTo))
            {
                PlannedDateTo = "01-01-0001";
            }

            if (String.IsNullOrEmpty(HarvestDate))
            {
                HarvestDate = "01-01-0001";
            }

            var culture = CultureInfo.CreateSpecificCulture("en-GB");
            dateParseStatus[0] = DateTime.TryParse(DateFrom, culture, DateTimeStyles.None, out searchDates[0]);
            dateParseStatus[1] = DateTime.TryParse(DateTo, culture, DateTimeStyles.None, out searchDates[1]);

            dateParseStatus[2] = DateTime.TryParse(PlannedDateFrom, culture, DateTimeStyles.None, out searchDates[2]);
            dateParseStatus[3] = DateTime.TryParse(PlannedDateTo, culture, DateTimeStyles.None, out searchDates[3]);
            dateParseStatus[4] = DateTime.TryParse(HarvestDate, culture, DateTimeStyles.None, out searchDates[4]);

            yield break;
        }
    }
}