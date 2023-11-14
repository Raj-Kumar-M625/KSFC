using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.Enquiry
{
    public class SecurityDocumentsDTO
    {
        public List<SecurityDetailsDTO> ListScrtyDtls { get; set; }
        public string TotalValue { get; set; }
        public DocumentsDTO Docs { get; set; }
    }
}
