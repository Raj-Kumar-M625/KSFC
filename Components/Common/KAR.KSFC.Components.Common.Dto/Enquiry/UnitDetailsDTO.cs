using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.Enquiry
{
    public class UnitDetailsDTO
    {
        public BasicDetailsDTO BasicDetails { get; set; }
        public List<AddressDTO> ListAddressDetail { get; set; }
        public BankDetailsDTO BankDetails { get; set; }
        public List<RegistrationDetailsDTO> ListRegDetails { get; set; }
    }
}
