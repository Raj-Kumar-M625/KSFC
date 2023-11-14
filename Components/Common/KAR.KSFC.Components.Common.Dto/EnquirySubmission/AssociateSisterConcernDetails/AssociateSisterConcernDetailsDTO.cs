using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.AssociateSisterConcernDetails
{
    public class AssociateSisterConcernDetailsDTO
    {
        public List<SisterConcernDetailsDTO> ListAssociates { get; set; }
        public List<SisterConcernFinancialDetailsDTO> ListFYDetails { get; set; }
    }
}
