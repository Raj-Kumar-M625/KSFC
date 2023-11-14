using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class FarmerSummaryReportModel
    {
        public long AgreementId { get; set; }

        [Display(Name = "Agreement Number")]
        public string AgreementNumber { get; set; }

        [Display(Name = "Farmer Name")]
        public string EntityName { get; set; }

        [Display(Name = "Aadhar Number")]
        public string UniqueId { get; set; }

        public string Crop { get; set; }

        [Display(Name = "Season Name")]
        public string SeasonName { get; set; }
        public string HQCode { get; set; }
    }
}