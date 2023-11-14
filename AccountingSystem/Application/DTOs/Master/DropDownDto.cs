using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Master
{
    public class DropDownDto
    {
        public int Id { get; set; }
        public string CodeType { get; set; }
        public string CodeName { get; set; }
        public string CodeValue { get; set; }
        public int DisplaySequence { get; set; }
        public IEnumerable<DropDownDto> CategoryType { get; set; }
        public IEnumerable<DropDownDto> PaymentTermsType { get; set; }
        public IEnumerable<DropDownDto> PaymentMethodType { get; set; }
        public IEnumerable<DropDownDto> GSTRegistrationType { get; set; }
        public IEnumerable<DropDownDto> TDSSectionType { get; set; }

        public IEnumerable<DropDownDto> VendorDocumentType { get; set; }
        public IEnumerable<DropDownDto> Cities { get; set; }
        public IEnumerable<DropDownDto> States { get; set; }
        public IEnumerable<DropDownDto> Country { get; set; }
        public IEnumerable<DropDownDto> BillDocumentType { get; set; }
        public IEnumerable<DropDownDto> CBF { get; set; }
        public IEnumerable<DropDownDto> LabourWelfareCess { get; set; }



    }
}
