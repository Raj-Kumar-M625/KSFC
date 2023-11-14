using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class DuplicateFarmersReport
    {
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string EntityName { get; set; }
        public string FatherHusbandName { get; set; }
        public string EntityNumber { get; set; }
        public bool IsActive { get; set; }
        public int AgreementCount { get; set; }
        public string UniqueId { get; set; }

        public string TerritoryName { get; set; }
        public string TerritoryCode { get; set; }

        public string HQName { get; set; }
        public string HQCode { get; set; }
        public string EntityType { get; set; }
    }
}
