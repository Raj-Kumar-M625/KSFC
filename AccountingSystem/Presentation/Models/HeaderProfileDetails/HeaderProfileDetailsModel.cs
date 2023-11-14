using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models.Profile
{
    public class HeaderProfileDetailsModel
    {
        [Key]
        public int Id { get; set; }
        public string EntityType { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string Extension { get; set; }
        public string ImagePath { get; set; }
        public string UploadedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public List<string>? DocumentName { get; set; }

        [Required]
        public List<IFormFile> files { get; set; }
        public bool IsActive { get; set; }
        public string Type { get; set; }

    }
}
