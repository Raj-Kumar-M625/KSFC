using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.UnitDetails
{
    public class UnitDetailsDTO
    {
        public BasicDetailsDto BasicDetails { get; set; }
        public List<AddressDetailsDTO> ListAddressDetail { get; set; }
        public BankDetailsDTO BankDetails { get; set; }
        public List<RegistrationNoDetailsDTO> ListRegDetails { get; set; }
    }
}
