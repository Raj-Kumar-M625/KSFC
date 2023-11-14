using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SqliteDomainWorkFlowPageData
    {
        public long EntityId { get; set; }
        public string EntityType { get; set; }
        public string EntityName { get; set; }
        public long AgreementId { get; set; }
        public string Agreement { get; set; }
        public long EntityWorkFlowDetailId { get; set; }

        public DateTime TimeStamp { get; set; }  // only date part is relevant

        public string TypeName { get; set; }
        public string TagName { get; set; }
        public string CurrentPhase { get; set; }

        public bool PhaseStarted { get; set; }
        public DateTime PhaseDate { get; set; }

        // fields for sowing
        public string SeedType { get; set; }
        public int SeedQuantity { get; set; }
        public bool GapFillingRequired { get; set; }
        public int GapFillingSeedQuantity { get; set; }

        // fields for First Harvest
        public int HarvestLaborCount { get; set; }

        // fields for other
        public int PercentCompleted { get; set; }

        public string ActivityId { get; set; }

        public IEnumerable<SqliteDomainWorkFlowFollowUp> FollowUps { get; set; }

        // April 11 2020
        public string BatchNumber { get; set; }
        public string LandSize { get; set; }
        public int ItemCount { get; set; }
        public string DWSEntry { get; set; }
        public IEnumerable<string> ItemsUsed { get; set; }
        public int YieldExpected { get; set; }
        public int BagsIssued { get; set; }
        public DateTime HarvestDate { get; set; }
    }
}
