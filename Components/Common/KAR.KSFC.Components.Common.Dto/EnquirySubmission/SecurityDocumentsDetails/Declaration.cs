using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.SecurityDocumentsDetails
{
    public class Declaration
    {
        [DisplayName("Security Id")]
        public int EnqSecId { get; set; }
 
         [DisplayName("Enquiry Id")]
        public int? EnqtempId { get; set; }
 
         [DisplayName("Declaration Date")]
         [Required(ErrorMessage = "The Declaration Date is required")]
         [DataType(DataType.Date)]
         [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? date { get; set; }
 
         [DisplayName("Place")]
         [Required(ErrorMessage = "Place is required")]
        public string Place { get; set; }
 
 
         [DisplayName("Name of Applicant")]
         [Required(ErrorMessage = "Name of Applicant is required")]
        public string NameOfApplicant { get; set; }
 
 
         [DisplayName("Designation of Applicant")]
         [Required(ErrorMessage = "Designation of Applicant is required")]
        public string ApplicantDesignation { get; set; }
 
 
 
         [DisplayName("Unique Id")]
         public string UniqueId { get; set; }


    }

}
