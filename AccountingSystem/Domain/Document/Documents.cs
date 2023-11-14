using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Document
{
    public class Documents
    {
        [Key]
        public int Id { get; set; }
        public int DocumentRefID { get; set; }
        public string Name { get; set; }
        public string FileType { get; set; }
        public string Extension { get; set; }
        public string Description { get; set; }
        public string UploadedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string FilePath { get; set; }
        public string EntityType { get; set; }
        public bool Status { get; set; }

    }

}
