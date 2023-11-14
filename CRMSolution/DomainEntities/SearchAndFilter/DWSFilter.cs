using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class DWSFilter : BaseSearchCriteria
    {
        public bool ApplyClientNameFilter { get; set; }
        public string ClientName { get; set; }

        public bool ApplyAgreementNumberFilter { get; set; }
        public string AgreementNumber { get; set; }

        public bool ApplySTRNumberFilter { get; set; }
        public bool IsExactSTRNumberMatch { get; set; } = false;
        public string STRNumber { get; set; }

        public bool ApplyDWSNumberFilter { get; set; }
        public string DWSNumber { get; set; }

        public bool ApplyDateFilter { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public bool ApplyDWSStatusFilter { get; set; }
        public string DWSStatus { get; set; }

        public bool ApplyPaymentReferenceFilter { get; set; }
        public bool IsExactPaymentReferenceMatch { get; set; } = false;
        public string PaymentReference { get; set; }

        public bool ApplyAgreementIdFilter { get; set; }
        public long AgreementId { get; set; }

        public bool ApplyEntityIdFilter { get; set; }
        public long EntityId { get; set; }
    }
}
