using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteEntityDisplayData
    {
        public long Id { get; set; }
        [DisplayName("Batch Id")]
        public long BatchId { get; set; }
        [DisplayName("Employee Id")]
        public long EmployeeId { get; set; }
        [DisplayName("Phone DB Id")]
        public string PhoneDbId { get; set; }
        [DisplayName("At Business")]
        public bool AtBusiness { get; set; }
        [DisplayName  ("Consent")]
        public bool Consent { get; set; }  //Swetha Made changes on 24-11-2021
        [DisplayName("Business Type")]
        public string EntityType { get; set; }
        [DisplayName("Business  Name")]
        public string EntityName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        [DisplayName("Land Size")]
        public string LandSize { get; set; }
        [DisplayName("Timestamp")]
        public System.DateTime TimeStamp { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        [DisplayName("Location Task Status")]
        public string LocationTaskStatus { get; set; }
        [DisplayName("Location Exception")]
        public string LocationException { get; set; }
        public long MNC { get; set; }
        public long MCC { get; set; }
        public long LAC { get; set; }
        [DisplayName("Cell Id")]
        public long CellId { get; set; }
        [DisplayName("Is Processed")]
        public bool IsProcessed { get; set; }
        [DisplayName("Entity Id")]
        public long EntityId { get; set; }
        [DisplayName("Date Created")]
        public System.DateTime DateCreated { get; set; }
        [DisplayName("Date Updated")]
        public System.DateTime DateUpdated { get; set; }
        [DisplayName("Number Of Contacts")]
        public int NumberOfContacts { get; set; }
        [DisplayName("Number Of Crops")]
        public int NumberOfCrops { get; set; }
        [DisplayName("Images?")]
        public int NumberOfImages { get; set; }

        public string UniqueIdType { get; set; }
        public string UniqueId { get; set; }
        public string TaxId { get; set; }

        [DisplayName("Locations?")]
        public int NumberOfLocations { get; set; }

        [DisplayName("Derived Loc Source")]
        public string DerivedLocSource { get; set; }

        [DisplayName("Derived Latitude")]
        public decimal DerivedLatitude { get; set; }

        [DisplayName("Derived Longitude")]
        public decimal DerivedLongitude { get; set; }

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