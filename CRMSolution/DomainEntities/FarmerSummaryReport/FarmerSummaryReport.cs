using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class FarmerSummaryReport
    {
        public long AgreementId { get; set; }
        public string AgreementNumber { get; set; }
        public string EntityName { get; set; }
        public string UniqueId { get; set; }
        public string Crop { get; set; }
        public string SeasonName { get; set; }
        public string HQCode { get; set; }
    }
}
