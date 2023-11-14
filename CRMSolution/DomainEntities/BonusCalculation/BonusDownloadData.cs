using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class BonusDownloadData : BonusPaymentReferences
    {
        public string EntityName { get; set; }
        public decimal BonusPaid { get; set; }
        public decimal BonusRate { get; set; }
        public DateTime AgreementDate { get; set; }
        public string TypeName { get; set; }
        public string SeasonName { get; set; }
        public decimal NetWeight { get; set; }
    }
}
