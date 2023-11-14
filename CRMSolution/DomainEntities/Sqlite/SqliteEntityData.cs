using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SqliteEntityData
    {
        public long Id { get; set; }
        public long BatchId { get; set; }
        public long EmployeeId { get; set; }
        public string PhoneDbId { get; set; }
        public bool AtBusiness { get; set; }
        //public bool Consent { get; set; }  //Swetha Made change on 24-11-2021
        public string EntityType { get; set; }
        public string EntityName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public string LandSize { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string LocationTaskStatus { get; set; }
        public string LocationException { get; set; }
        public long MNC { get; set; }
        public long MCC { get; set; }
        public long LAC { get; set; }
        public long CellId { get; set; }
        public bool IsProcessed { get; set; }
        public long EntityId { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateUpdated { get; set; }
        public int NumberOfContacts { get; set; }
        public int NumberOfCrops { get; set; }
        public int NumberOfImages { get; set; }

        public string UniqueIdType { get; set; }
        public string UniqueId { get; set; }
        public string TaxId { get; set; }
        public int NumberOfLocations { get; set; }

        public string DerivedLocSource { get; set; }
        public decimal DerivedLatitude { get; set; }
        public decimal DerivedLongitude { get; set; }

        public IEnumerable<SqliteEntityLocationData> Locations { get; set; }

        public string FatherHusbandName { get; set; }
        public string TerritoryCode { get; set; }
        public string TerritoryName { get; set; }
        public string HQCode { get; set; }
        public string HQName { get; set; }
        //public string MajorCrop { get; set; }
        //public string LastCrop { get; set; }
        //public string WaterSource { get; set; }
        //public string SoilType { get; set; }
        //public string SowingType { get; set; }
        //public DateTime SowingDate { get; set; }
    }
}
