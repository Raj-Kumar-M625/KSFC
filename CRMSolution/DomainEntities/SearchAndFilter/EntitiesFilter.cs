using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class EntitiesFilter : BaseSearchCriteria
    {
        public bool ApplyClientNameFilter { get; set; }
        public string ClientName { get; set; }

        public bool ApplyClientTypeFilter { get; set; }
        public string ClientType { get; set; }

        public bool ApplyEmployeeCodeFilter { get; set; }
        public string EmployeeCode { get; set; }

        public bool ApplyEmployeeNameFilter { get; set; }
        public string EmployeeName { get; set; }

        public bool ApplyAgreementStatusFilter { get; set; }
        public string AgreementStatus { get; set; }

        public bool ApplyAgreementNumberFilter { get; set; }
        public string AgreementNumber { get; set; }

        public bool ApplyUniqueIdFilter { get; set; }
        public string UniqueId { get; set; }

        public bool ApplyCropFilter { get; set; }
        public string Crop { get; set; }

        public bool ApplyActiveFilter { get; set; }
        public bool IsActive { get; set; }

        public bool ApplyBankDetailStatusFilter { get; set; }
        public string BankDetailStatus { get; set; }

        public bool ApplyEntityNumberFilter { get; set; }
        public string EntityNumber { get; set; }
    }
}
