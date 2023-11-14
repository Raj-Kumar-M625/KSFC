using KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails
{
    public class GuarantorAssetsNetWorthDTO
    {

        [DisplayName("Asset Id")]
        public int? EnqGuarassetId { get; set; }

        [DisplayName("Enquiry Id")]
        public int EnqtempId { get; set; }


        [DisplayName("Promoter Code")]
        [Required(ErrorMessage = "Guarantor is required")]
        public long? PromoterCode { get; set; }


        [DisplayName("Asset Category Code")]
        [Required(ErrorMessage = "Category is required")]
        public int AssetcatCd { get; set; }

        [DisplayName("Asset Type Code")]
        [Required(ErrorMessage = "Type is required")]
        public int AssettypeCd { get; set; }


        [DisplayName("Asset Description")]
        [Required(ErrorMessage = "Description is required")]
        public string GuarAssetDesc { get; set; }

        [DisplayName("Asset Value")]
        [Required(ErrorMessage = "Value is required")]
        public decimal? GuarAssetValue { get; set; }


        [DisplayName("Asset Site Number")]
        public string GuarAssetSiteno { get; set; }

        [DisplayName("Asset Address")]
        [Required(ErrorMessage = "Address is required")]
        public string GuarAssetAddr { get; set; }

        [DisplayName("Asset Dimension")]
        [Required(ErrorMessage = "Dimension is required")]
        public string GuarAssetDim { get; set; }


        [DisplayName("Asset Area")]
        [Required(ErrorMessage = "Area is required")]
        public decimal? GuarAssetArea { get; set; }


        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }

        public int? EnqGuarId { get; set; }
        public virtual GuarantorDetailsDTO GuarantorDetailsDTO { get; set; }
        public virtual AssetCategoryMasterDTO AssetCategoryMasterDTO { get; set; }

        public virtual AssetTypeMasterDTO AssetTypeMasterDTO { get; set; }

    }
}
