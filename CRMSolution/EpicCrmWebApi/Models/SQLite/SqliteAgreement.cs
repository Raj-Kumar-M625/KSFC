using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteAgreement
    {
        public string Id { get; set; }
        public bool IsNewEntity { get; set; }
        public long EntityId { get; set; }
        public string EntityName { get; set; }

        public string ParentReferenceId { get; set; }  // fk to ServerEntity.Id or 

        // if entityId is zero, then use ReferenceId as fk in Entity table
        // else entityId is fk in ServerEntity

        public string SeasonName { get; set; }
        public string TypeName { get; set; }  // crop name goes here
        public double Acreage { get; set; }

        public DateTime TimeStamp { get; set; }  // only date part is relevant
        public string ActivityId { get; set; }
        public string TerritoryCode { get; set; }
        public string TerritoryName { get; set;}
        public string HQCode { get; set; }  
        public string HQName { get; set;}
    }
}