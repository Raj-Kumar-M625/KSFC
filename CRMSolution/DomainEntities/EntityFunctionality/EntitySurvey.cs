using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class EntitySurvey
    {
        public long Id { get; set; }
        public long EntityId { get; set; }
        public long WorkflowSeasonId { get; set; }
        public string WorkflowSeasonName { get; set; }
        public string TypeName { get; set; }
        public string SurveyNumber { get; set; }
        public string Status { get; set; }

        public string MajorCrop { get; set; }
        public string LastCrop { get; set; }
        public string WaterSource { get; set; }
        public string SoilType { get; set; }
        public Nullable<System.DateTime> SowingDate { get; set; }
        public decimal LandSizeInAcres { get; set; }

        public long ActivityId { get; set; }

        public int ActivityCount { get; set; }
    }
}
