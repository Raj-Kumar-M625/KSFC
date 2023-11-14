using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters
{
    public class AssetTypeMasterDTO
    {

        [DisplayName("Asset Type Code")]
        [Required(ErrorMessage = "Asset Type Code is required")]
        public int AssettypeCd { get; set; }

        [DisplayName("Asset Type Details")]
        [Required(ErrorMessage = "Asset Type Details are required")]
        public string AssettypeDets { get; set; }


        [DisplayName("Asset Category Code")]
        [Required(ErrorMessage = "Asset Category Code is required")]
        public int AssetcatCd { get; set; }

        [DisplayName("Sequence Number")]

        public decimal? SeqNo { get; set; }


        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }
    }
}
