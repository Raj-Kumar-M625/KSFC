using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class BonusCalculationFilter : BaseSearchCriteria
    {
               
        public string ClientName { get; set; }

        public string AgreementNumber { get; set; }

        public string DateFrom { get; set; }

        public string DateTo { get; set; }

        public bool ApplyBonusStatusFilter { get; set; }
        public string BonusStatus { get; set; }

        public bool ApplySeasonNameFilter { get; set; }
        public string SeasonName { get; set; }
    }
}