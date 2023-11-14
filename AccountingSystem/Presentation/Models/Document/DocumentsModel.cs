using Domain.Vendor;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models
{
    public class DocumentsModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int DocumentRefID { get; set; }
        public string Name { get; set; }
        public string FileType { get; set; }
        public string Extension { get; set; }
        public string Description { get; set; }
        public string UploadedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }

        [Required]
        public List<IFormFile> files { get; set; }

        public List<string>? DocumentName { get; set; }
        public string FilePath { get; set; }
        public string EntityType { get; set; }
        public bool Status { get; set; }
    }
}
