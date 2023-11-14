
using System;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission
{
    public class EnquirySummary
    {
        public int? EnquiryId { get; set; }
        public int? EnqStatus { get; set; }
        public string PromotorPan { get; set; }
        public DateTime? EnqInitiateDate { get; set; }
        public DateTime? EnqSubmitDate { get; set; }

    }
}
