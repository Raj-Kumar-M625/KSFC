using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class EntityProgressDetail
    {
        public long Id { get; set; }

        public long EntityId { get; set; }

        public string EntityName { get; set; }

        public long EmployeeId { get; set; }

        public string EmployeeCode { get; set; }

        public string EmployeeName { get; set; }

        public string TypeName { get; set; }
        public string SeasonName { get; set; }

        public string LastPhase { get; set; }

        public DateTime? LastPhaseDate { get; set; }

        public string CurrentPhase { get; set; }

        public DateTime CurrentPlannedFromDate { get; set; }

        public DateTime CurrentPlannedEndDate { get; set; }

        public string HQCode { get; set; }

        public bool IsComplete { get; set; }

        public string AgreementNumber { get; set; }
        public string AgreementStatus { get; set; }
    }
}
