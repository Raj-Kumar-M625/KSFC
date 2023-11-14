using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters
{
    public class BuildingTypeMasterDTO
    {
        [DisplayName("Building Code")]
        [Required(ErrorMessage = "The Building code is required")]
        public short BldCd { get; set; }

        [DisplayName("Building Description")]

        [Required(ErrorMessage = "The Building Description is required")]
        public string BldDesc { get; set; }

        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }
    }
}
