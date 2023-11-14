using KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails
{
    public class PromoterAssetsNetWorthDTO 
    {


        [DisplayName("Promoter Assets")]
        public int? EnqPromassetId { get; set; }
 
        [DisplayName("Enquiry Id")]
        public int EnqtempId { get; set; }

        [DisplayName("Promoter Code")]
        [Required(ErrorMessage = "Promoter is required")]
        public long? PromoterCode { get; set; }

        [DisplayName("Asset Category Code")]
        [Required(ErrorMessage = "Category is required")]
        public int AssetcatCd { get; set; }


        [DisplayName("AssetType Code")]
        [Required(ErrorMessage = "Type is required")]
        public int AssettypeCd { get; set; }

        [DisplayName("Asset Description")]
        [Required(ErrorMessage = "Description is required")]
        public string EnqAssetDesc { get; set; }


        [DisplayName("Asset Value")]
        [Required(ErrorMessage = "Value is required")]
        public decimal? EnqAssetValue { get; set; }


        [DisplayName("Asset Site Number")]
        [Required(ErrorMessage = "Site no is required")]
        public string EnqAssetSiteno { get; set; }


        [DisplayName("Asset Address")]
        [Required(ErrorMessage = "Address is required")]
        public string EnqAssetAddr { get; set; }



        [DisplayName("Asset Dimension")]
        [Required(ErrorMessage = "Dimension is required")]
        public string EnqAssetDim { get; set; }



        [DisplayName("Asset Area")]
        [Required(ErrorMessage = "Area is required")]
        public decimal? EnqAssetArea { get; set; }


        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }

        public int? EnqPromId { get; set; }
        public virtual PromoterMasterDTO PromoterMasterDTO { get; set; }

        public virtual AssetCategoryMasterDTO AssetCategoryMasterDTO { get; set; }

        public virtual AssetTypeMasterDTO AssetTypeMasterDTO { get; set; }
    }
}
