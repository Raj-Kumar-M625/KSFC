using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.ProjectDetails
{
    public class ProjectEquityDebitDetailsDTO
    {

        [DisplayName("Means Of Finanace Id")]
        public int EnqMftotalId { get; set; }

        [DisplayName("Enquiry Id")]
        public int? EnqtempId { get; set; }


        [DisplayName("Equity")]
        public decimal? EnqEquity { get; set; }

        [DisplayName("Debt")]
        public decimal? EnqDebt { get; set; }


        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }

    }
}
