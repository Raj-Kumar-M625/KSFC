using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters
{
    public class IndusProductMasterDTO
    {

        [DisplayName("Industry Code")]

        [Required(ErrorMessage = "The Industry Code is required")]
        public int ProdInd { get; set; }

        [DisplayName("Product Code")]

        [Required(ErrorMessage = "The Product Code is required")]
        public int ProdCd { get; set; }

        [DisplayName("Product Details")]

        [Required(ErrorMessage = "The Product Details is required")]
        public string ProdDets { get; set; }

        [DisplayName("Department")]
        [Required(ErrorMessage = "The Department is required")]
        public string Dept { get; set; }

        [DisplayName("Product National Code")]
        [Required(ErrorMessage = "The Product NationalCode is required")]
        public int? ProdNcd { get; set; }

        [DisplayName("Product National Description")]
        [Required(ErrorMessage = "The Product NationalDescription is required")]
        public string ProdNdt { get; set; }

        [DisplayName("Product Flag")]
        public int? ProfFlg { get; set; }

        [DisplayName("Id")]
        public int Id { get; set; }
    }
}
