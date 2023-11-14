using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteSurveyDisplayData
    {
        public long Id { get; set; }
        public bool IsProcessed { get; set; }
        public long EntitySurveyId { get; set; }

        public bool IsNewEntity { get; set; }
        public long EntityId { get; set; }
        public string EntityName { get; set; }

        // used only for new entries to match it up on server
        public string ParentReferenceId { get; set; }

        public string SeasonName { get; set; }
        public string SowingType { get; set; }  // crop name goes here
        public double Acreage { get; set; }

        public DateTime SowingDate { get; set; }
        public string MajorCrop { get; set; }
        public string LastCrop { get; set; }
        public string WaterSource { get; set; }
        public string SoilType { get; set; }


        public DateTime TimeStamp { get; set; }  // only date part is relevant
        public string ActivityId { get; set; }

        public string PhoneDbId { get; set; }
    }
}