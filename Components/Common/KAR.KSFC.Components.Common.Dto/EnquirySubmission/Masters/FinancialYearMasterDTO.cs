using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters
{
    public class FinancialYearMasterDTO
    {

        [DisplayName(" Financial Year Code")]
        public int FinyearCode { get; set; }

        [DisplayName("Financial Year Description")]
        public string FinyearDesc { get; set; }


        [DisplayName("From Date")]
        public DateTime? FromDate { get; set; }

        [DisplayName("To Date")]
        public DateTime? ToDate { get; set; }

        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }
    }
}
