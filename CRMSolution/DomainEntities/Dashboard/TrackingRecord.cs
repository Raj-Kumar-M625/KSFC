using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class TrackingRecord
    {
        [Display(Name ="Record Id")]
        public long Id { get; set; }

        public long? ChainedTrackingId { get; set; }

        public long EmployeeDayId { get; set; }

        public System.DateTime At { get; set; }

        [Display(Name ="Start Position")]
        public Decimal BeginLatitude { get; set; }
        public Decimal BeginLongitude { get; set; }

        [Display(Name = "Location")]
        public Decimal EndLatitude { get; set; }
        public Decimal EndLongitude { get; set; }

        public string BeginLocationName { get; set; }
        [Display(Name ="Location")]
        public string EndLocationName { get; set; }

        [Display(Name = "Bing")]
        public Decimal BingMapsDistanceInMeters { get; set; }
        [Display(Name = "Google")]
        public Decimal GoogleMapsDistanceInMeters { get; set; }
        [Display(Name = "Raw")]
        public Decimal LinearDistanceInMeters { get; set; }

        [Display(Name ="Calculated?")]
        public bool DistanceCalculated { get; set; }

        [Display(Name = "Locked At")]
        public Nullable<DateTime> LockTimeStamp { get; set; }

        public long BingApiErrorId { get; set; }
        public long GoogleApiErrorId { get; set; }

        public long ActivityId { get; set; }
        public bool IsStartOfDay { get; set; }
        public bool IsEndOfDay { get; set; }

        public string ActivityType { get; set; }

        public string BeginCoorindates => $"{BeginLatitude.ToString()}, {BeginLongitude.ToString()}";
        public string EndCoorindates => $"{EndLatitude.ToString()}, {EndLongitude.ToString()}";
        public string DisplayRecordId => $"{Id} | {ChainedTrackingId?.ToString()}";
        public string DisplayLockTime => LockTimeStamp.HasValue ? $"{LockTimeStamp.Value.ToString("MMM dd, yyyy hh:mm:ss")}" : "";
    }
}
