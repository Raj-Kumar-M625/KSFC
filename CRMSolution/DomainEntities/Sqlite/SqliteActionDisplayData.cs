using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SqliteActionDisplayData
    {
        public long Id { get; set; }

        [Display(Name="Mob Table Id")]
        public string PhoneDbId { get; set; }
        public long BatchId { get; set; }
        public long EmployeeId { get; set; }
        public System.DateTime At { get; set; }

        //public string AtAsString => String.Format("{0:yyyy-MM-dd hh:mm:ss tt}", At);

        public string Name { get; set; }

        [Display(Name = "Type")]
        public int ActivityTrackingType { get; set; }

        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public long MNC { get; set; }
        public long MCC { get; set; }
        public long LAC { get; set; }
        public long CellId { get; set; }

        [Display(Name = "Client Name")]
        public string ClientName { get; set; }
        [Display(Name = "Client Phone")]
        public string ClientPhone { get; set; }
        [Display(Name = "Client Type")]
        public string ClientType { get; set; }
        [Display(Name = "Activity Type")]
        public string ActivityType { get; set; }
        public string Comments { get; set; }

        [Display(Name = "Images?")]
        public int ImageCount { get; set; }

        [Display(Name = "Processed?")]
        public bool IsProcessed { get; set; }
        public bool IsPostedSuccessfully { get; set; }
        public long TrackingId { get; set; }
        public long ActivityId { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateUpdated { get; set; }

        public string PhoneModel { get; set; }
        public string PhoneOS { get; set; }
        public string AppVersion { get; set; }

        [Display(Name = "Client Code")]
        public string ClientCode { get; set; }

        public string IMEI { get; set; }

        [Display(Name = "Contact Count")]
        public int ContactCount { get; set; }

        [Display(Name = "At Business")]
        public bool AtBusiness { get; set; }

        [Display(Name= "Instrument Id")]
        public string InstrumentId { get; set; }

        [Display(Name = "Activity Amount")]
        public decimal ActivityAmount { get; set; }

        [Display(Name = "Location Task Status")]
        public string LocationTaskStatus { get; set; }

        [Display(Name = "Location Exception")]
        public string LocationException { get; set; }

        [Display(Name = "Location Count")]
        public int LocationCount { get; set; }

        [Display(Name = "Derived Loc Source")]
        public string DerivedLocSource { get; set; }

        [Display(Name = "Derived Latitude")]
        public decimal DerivedLatitude { get; set; }

        [Display(Name = "Derived Longitude")]
        public decimal DerivedLongitude { get; set; }
    }
}
