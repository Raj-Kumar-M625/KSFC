using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteAction : SqliteBase
    {
        public string Id { get; set; }
        public string ActivityType { get; set; }
        public string ClientType { get; set; }
        public string ClientName { get; set; }
        public string ClientPhone { get; set; } // 1.7k onwards, this will be blank for type 4
                                                // instead use Contacts
        public string Comments { get; set; }
        public DateTime TimeStamp { get; set; }
        public int ActivityTrackingType { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public long MNC { get; set; }
        public long MCC { get; set; }
        public long LAC { get; set; }
        public long CellId { get; set; }

        // bool to indicate if sales person is at business site while capturing an activity
        // this field has relevance only for ActivityTrackingType = 4
        public bool AtBusiness { get; set; }

        public string LocationTaskStatus { get; set; }
        public string LocationException { get; set; }
        public string ClientCode { get; set; }
        public string InstrumentId { get; set; }
        public decimal ActivityAmount { get; set; }

        public IEnumerable<SqliteContact> Contacts { get; set; }
        public IEnumerable<SqliteLocation> Locations { get; set; }
    }
}