using System;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters
{
    public class LoanPurposeMasterDTO
    {
        [DisplayName("Loan Purpose Code")]
        [Required(ErrorMessage = "The Loan Purpose Code is required")]
        public int PurpCd { get; set; }

        [DisplayName("Loan Purpose Details")]
        [Required(ErrorMessage = "The Loan Purpose Detail is required")]
        public string PurpDets { get; set; }


        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }
    }
}
