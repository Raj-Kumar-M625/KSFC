using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters
{
    public class BankFacilityMasterDTO
    {
        [DisplayName("Bank Facility Code")]
        public int BfacilityCode { get; set; }
        [DisplayName("Bank Facility Description")]
        public string BfacilityDesc { get; set; }

        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }
    }
}
