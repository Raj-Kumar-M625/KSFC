using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Master
{
    public class CommonMasterDto
    {
        public int Id { get; set; }
        public string CodeType { get; set; }
        public string CodeName { get; set; }
        public string CodeValue { get; set; }
        public int DisplaySequence { get; set; }
       
    }
}
