
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters
{
    public class PromoterClassMasterDTO
    {
        [DisplayName("Promoter Class Code")]
         [Required(ErrorMessage = "The Promoter Class Code is required")]
        public int PclasCd { get; set; }
 
 
         [DisplayName("Promoter Class Details")]
         [Required(ErrorMessage = "The Promoter Class Details is required")]
        public string PclasDets { get; set; }
 
         [DisplayName("Id")]
        public int Id { get; set; }
    }
}
