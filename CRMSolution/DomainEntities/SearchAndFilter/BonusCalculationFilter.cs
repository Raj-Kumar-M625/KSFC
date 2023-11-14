using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class BonusCalculationFilter : BaseSearchCriteria
    {
        public bool ApplyClientNameFilter { get; set; }
        public string ClientName { get; set; }
       
        public bool ApplyAgreementNumberFilter { get; set; }
        public string AgreementNumber { get; set; }
       
        public bool ApplyDateFilter { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
      
        public bool ApplyBonusStatusFilter { get; set; }
        public string BonusStatus { get; set; }

        public bool ApplySeasonNameFilter { get; set; }
        public string SeasonName { get; set; }
    }
}