using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters
{
    public class IndusSectorMasterDTO
    {


        [DisplayName("Sector Code")]
        public short SecCd { get; set; }

        [DisplayName("Sector Details")]
        public string SecDets { get; set; }

        [DisplayName("Sector Details Flag")]
        public byte? SecTy { get; set; }


        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }
    }
}
