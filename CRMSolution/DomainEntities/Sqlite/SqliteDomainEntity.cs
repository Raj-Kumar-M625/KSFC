using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SqliteDomainEntity
    {
        public string PhoneDbId { get; set; }
        public bool AtBusiness { get; set; }
        //public bool Consent { get; set; } //Swetha Made change on 24-11-2021
        public string EntityType { get; set; }
        public string EntityName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public string LandSize { get; set; }
        public DateTime TimeStamp { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string LocationTaskStatus { get; set; }
        public string LocationException { get; set; }
        public long MNC { get; set; }
        public long MCC { get; set; }
        public long LAC { get; set; }
        public long CellId { get; set; }

        public IEnumerable<SqliteDomainEntityContact> Contacts { get; set; }
        public IEnumerable<SqliteDomainEntityCrop> Crops { get; set; }
        public IEnumerable<String> Images { get; set; }

        public string UniqueIdType { get; set; }
        public string UniqueId { get; set; }
        public string TaxId { get; set; }
        public IEnumerable<SqliteDomainActionLocation> Locations { get; set; }

        public string FatherHusbandName { get; set; }
        public string TerritoryCode { get; set; }
        public string TerritoryName { get; set; }
        public string HQCode { get; set; }
        public string HQName { get; set; }
    }
}
