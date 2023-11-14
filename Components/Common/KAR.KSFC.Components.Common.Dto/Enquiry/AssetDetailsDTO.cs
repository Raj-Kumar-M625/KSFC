using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.Enquiry
{
    public class AssetDetailsDTO
    {
        [Display(Name = "S. No.")]
        public int? Id { get; set; }
        [Display(Name = "Name")]

        [MaxLength(30)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public int? NameId { get; set; }
        [Display(Name = "Asset Category")]
        public string AssetCategory { get; set; }
        [Display(Name = "Asset Type")]
        public string AssetType { get; set; }
        [Display(Name = "Survey / Site / Flat No")]
        [MaxLength(10)]
        public string FlatNumber { get; set; }
        [Display(Name = "Site / Flat Address")]
        [MaxLength(40)]
        public string FlatAddress { get; set; }
        [Display(Name = "Dimension")]
        [MaxLength(10)]
        public string Dimension { get; set; }
        [Display(Name = "Area")]
        [MaxLength(10)]
        public string Area { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Value")]
        [MaxLength(20)]
        [Required(ErrorMessage = "Value is required.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Invalid Value.")]
        public string Value { get; set; }
        [Display(Name = "Mode of Acquire")]
        public string AcquireMode { get; set; }

    }
}
