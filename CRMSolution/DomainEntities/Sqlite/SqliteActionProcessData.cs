using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SqliteActionProcessData
    {
        public long Id { get; set; }
        public long SqliteTableId { get; set; }
        public long BatchId { get; set; }
        public long EmployeeId { get; set; }
        public System.DateTime At { get; set; }
        //public string Name { get; set; }
        public int ActivityTrackingtype { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public long MNC { get; set; }
        public long MCC { get; set; }
        public long LAC { get; set; }
        public long CellId { get; set; }

        public string ClientName { get; set; }
        public string ClientPhone { get; set; }
        public string ClientType { get; set; }
        public string ActivityType { get; set; }
        public string Comments { get; set; }

        public int ImageCount { get; set; }
        //public IEnumerable<byte[]> Images { get; set; }
        public IEnumerable<string> Images { get; set; }

        public bool IsProcessed { get; set; }
        public bool IsPostedSuccessfully { get; set; }
        public long TrackingId { get; set; }
        public long ActivityId { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateUpdated { get; set; }

        public string PhoneModel { get; set; }
        public string PhoneOS { get; set; }
        public string AppVersion { get; set; }

        public string ClientCode { get; set; }
        public string IMEI { get; set; }
        public int ContactCount { get; set; }
        public bool AtBusiness { get; set; }
        public string InstrumentId { get; set; }
        public decimal ActivityAmount { get; set; }
        public int LocationCount { get; set; }
        public IEnumerable<SqliteActionContactData> Contacts { get; set; }
        public IEnumerable<SqliteActionLocationData> Locations { get; set; }

        public decimal DerivedLatitude { get; set; }
        public decimal DerivedLongitude { get; set; }
        public string DerivedLocSource { get; set; }
    }
}
