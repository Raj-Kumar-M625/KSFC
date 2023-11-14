using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.Enums
{
    public enum FileTypesEnum
    {
        [Display(Name = "PDF")]
        Pdf = 1,
        [Display(Name = "JPEG")]
        Jpeg = 2,
        [Display(Name = "JPG")]
        Jpg = 3
    }
}
