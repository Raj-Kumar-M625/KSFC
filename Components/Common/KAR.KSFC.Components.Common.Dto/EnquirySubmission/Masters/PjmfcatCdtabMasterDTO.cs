using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters
{
    public class PjmfcatCdtabMasterDTO
    {

        [DisplayName("Means Of Finance Code")]
        public int MfcatCd { get; set; }

        [DisplayName("Means Of Finance Details")]
        public string PjmfDets { get; set; }

        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }

    }
}
