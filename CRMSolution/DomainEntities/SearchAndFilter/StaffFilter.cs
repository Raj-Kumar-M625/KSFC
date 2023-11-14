using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class StaffFilter : BaseSearchCriteria
    {
        public bool ApplyEmployeeCodeFilter { get; set; }
        public string EmployeeCode { get; set; }

        public bool ApplyNameFilter { get; set; }
        public string Name { get; set; }

        public bool ApplyPhoneFilter { get; set; }
        public string Phone { get; set; }

        public bool ApplyGradeFilter { get; set; }
        public string Grade { get; set; }

        public bool ApplyStatusFilter { get; set; }
        public bool Status { get; set; }

        public bool ApplyAssociationFilter { get; set; }
        public bool Association { get; set; }

        public bool ApplyDepartmentFilter { get; set; }
        public string Department { get; set; }

        public bool ApplyDesignationFilter { get; set; }
        public string Designation { get; set; }
    }
}
