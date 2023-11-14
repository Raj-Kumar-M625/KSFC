using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters
{
    public class AssetCategoryMasterDTO
    {

        [DisplayName("Asset Category Code")]
        public int AssetcatCd { get; set; }

        [DisplayName("Asset Category Details")]
        public string AssetcatDets { get; set; }
         

        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }
    }
}
