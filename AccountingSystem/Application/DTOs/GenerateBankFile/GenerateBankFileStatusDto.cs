using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.GenerateBankFile
{
    public class GenerateBankFileStatusDto
    {
        public int Id { get; set; }
        public int GenerateBankFileId { get; set; }
        public int StatusCMID { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifedBy { get; set; }
    }
}
