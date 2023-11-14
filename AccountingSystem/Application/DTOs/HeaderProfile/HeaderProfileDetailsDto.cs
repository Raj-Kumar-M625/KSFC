using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Profile
{
    public  class HeaderProfileDetailsDto
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
        public List<IFormFile> files { get; set; }

        public bool IsActive { get; set; }
        public string Type { get; set; }






    }
}
