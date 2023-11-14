using System;
using System.ComponentModel;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters
{
    public class AddressTypeMasterDTO
    {

        [DisplayName("Address Type Code")]
        public int? AddtypeCd { get; set; }

        [DisplayName("Address Type Details")]
        public string AddtypeDets { get; set; }

        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }
    }
}
