using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.Enquiry
{
    public class AssociateSisterConcernDetailsDTO
    {
        public List<AssociateDetailsDTO> ListAssociates { get; set; }
        public List<PrevFYDetailsDTO> ListFYDetails { get; set; }
    }
}
