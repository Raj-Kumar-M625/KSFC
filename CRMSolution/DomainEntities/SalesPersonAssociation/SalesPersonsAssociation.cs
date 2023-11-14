using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SalesPersonsAssociation
    {
        public string StaffCode { get; set; }
        //public bool IsDeleted { get; set; }
        public DateTime AssignedDate { get; set; }

        public string CodeType { get; set; }
        public string CodeValue { get; set; }

        public string CodeName { get; set; } 
        public string Code { get; set; } 
        public bool IsAssigned { get; set; }
    }
}
