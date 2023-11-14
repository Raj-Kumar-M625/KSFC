using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteSurvey
    {
        public string Id { get; set; }
        public bool IsNewEntity { get; set; }
        public long EntityId { get; set; }
        public string EntityName { get; set; }

        public string ParentReferenceId { get; set; }  // fk to ServerEntity.Id or 

        // if entityId is zero, then use ReferenceId as fk in Entity table
        // else entityId is fk in ServerEntity

        public string SeasonName { get; set; }
        public string SowingType { get; set; }  // crop name goes here
        public double Acreage { get; set; }
        public string MajorCrop { get; set; }
        public string LastCrop { get; set; }
        public string WaterSource { get; set; }
        public string SoilType { get; set; }
        public DateTime SowingDate { get; set; }

        public DateTime TimeStamp { get; set; }  // only date part is relevant
        public string ActivityId { get; set; }
    }
}