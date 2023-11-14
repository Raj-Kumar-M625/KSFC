using Microsoft.AspNetCore.Http;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.Email
{
    public class EmailServiceRequest
    {
        [Required(ErrorMessage ="ToEmail Address is required")]
        public string ToEmail { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }

}
