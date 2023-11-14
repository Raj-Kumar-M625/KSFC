using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class Entity
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public long DayId { get; set; }
        public string HQCode { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public bool AtBusiness { get; set; }
        public bool Consent { get; set; } //swetha made changes on 24-11
        public string EntityType { get; set; }
        public string EntityName { get; set; }
        public System.DateTime EntityDate { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public string LandSize { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public long SqliteEntityId { get; set; }
        public int ContactCount { get; set; }
        public int CropCount { get; set; }
        public int ImageCount { get; set; }
        public int AgreementCount { get; set; }
        public int BankDetailCount { get; set; }
        public int SurveryDetailCount { get; set; }

        public string UniqueIdType { get; set; }
        public string UniqueId { get; set; }
        public string TaxId { get; set; }

        public string FatherHusbandName { get; set; }
        public string HQName { get; set; }
        public string TerritoryCode { get; set; }
        public string TerritoryName { get; set; }
        //public string MajorCrop { get; set; }
        //public string LastCrop { get; set; }
        //public string WaterSource { get; set; }
        //public string SoilType { get; set; }
        //public string SowingType { get; set; }
        //public Nullable<System.DateTime> SowingDate { get; set; }
        public string EntityNumber { get; set; }
        public bool IsActive { get; set; }

        public string CurrentUser { get; set; }

        // May 30 2020
        public int DWSCount { get; set; }
        public int IssueReturnCount { get; set; }
        public int AdvanceRequestCount { get; set; }
        public string CreatedBy { get; set; }

    }
}
