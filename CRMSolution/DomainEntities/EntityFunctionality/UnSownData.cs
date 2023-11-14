using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class UnSownData
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public long DayId { get; set; }
        public string HQCode { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string EntityType { get; set; }
        public string EntityName { get; set; }
        public string LandSize { get; set; }

        public long SqliteEntityId { get; set; }

        public string UniqueIdType { get; set; }
        public string UniqueId { get; set; }
        
        public string CurrentUser { get; set; }
    }
}
