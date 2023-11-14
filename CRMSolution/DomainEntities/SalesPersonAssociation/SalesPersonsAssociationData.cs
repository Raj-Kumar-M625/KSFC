using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SalesPersonsAssociationData
    {
        public string CodeType { get; set; }
        public string CodeName { get; set; }
        public string Code { get; set; }
        public DateTime AssignedDate { get; set; }
    }

    public class SalesPersonsAssociationDataForAll : SalesPersonsAssociationData
    {
        public string StaffCode { get; set; }
    }
}
