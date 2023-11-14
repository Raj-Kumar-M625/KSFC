using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SqliteDomainSurvey
    {
        public string PhoneDbId { get; set; }
        public bool IsNewEntity { get; set; }
        public long EntityId { get; set; }
        public string EntityName { get; set; }

        public string ParentReferenceId { get; set; }  // fk to ServerEntity.Id or 

        // if entityId is zero, then use ReferenceId as fk in Entity table
        // else entityId is fk in ServerEntity

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
    }

    public class SqliteSurveyData : SqliteDomainSurvey
    {
        public long Id { get; set; }
        public long BatchId { get; set; }
        public long EmployeeId { get; set; }
        public bool IsProcessed { get; set; }
        public long EntitySurveyId { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateUpdated { get; set; }
    }
}
