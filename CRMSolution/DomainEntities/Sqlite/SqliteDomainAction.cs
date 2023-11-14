using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SqliteDomainAction
    {
        public long Id { get; set; }
        public string PhoneDbId { get; set; }
        public DateTime TimeStamp { get; set; }
        public int ActivityTrackingType { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public long MNC { get; set; }
        public long MCC { get; set; }
        public long LAC { get; set; }
        public long CellId { get; set; }
        public string ActivityType { get; set; }
        public string ClientType { get; set; }
        public string ClientName { get; set; }
        public string ClientPhone { get; set; }
        public string Comments { get; set; }
        public IEnumerable<string> Images { get; set; }

        public bool AtBusiness { get; set; }

        public string LocationTaskStatus { get; set; }
        public string LocationException { get; set; }
        public string ClientCode { get; set; }
        public string InstrumentId { get; set; }
        public decimal ActivityAmount { get; set; }
        public IEnumerable<SqliteDomainActionContact> Contacts { get; set; }
        public string IMEI { get; set; }

        public string PhoneModel { get; set; }
        public string PhoneOS { get; set; }
        public string AppVersion { get; set; }
        public IEnumerable<SqliteDomainActionLocation> Locations { get; set; }
    }
}
