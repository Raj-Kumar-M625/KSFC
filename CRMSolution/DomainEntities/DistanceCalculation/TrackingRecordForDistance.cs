using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class TrackingRecordForDistance
    {
        public long Id { get; set; }
        public Decimal BeginLatitude { get; set; }
        public Decimal BeginLongitude { get; set; }
        public Decimal EndLatitude { get; set; }
        public Decimal EndLongitude { get; set; }
        public Decimal BingMapsDistanceInMeters { get; set; } = 0;
        public Decimal GoogleMapsDistanceInMeters { get; set; } = 0;
        public Decimal LinearDistanceInMeters { get; set; } = 0;
        public bool IsMileStone { get; set; }
        public bool IsStartOfDay { get; set; }
        public bool IsEndOfDay { get; set; }

        public bool BingMapError { get; set; } = false;
        public bool GoogleMapError { get; set; } = false;

        public string BingMapErrorDetails { get; set; } = "";
        public string GoogleMapErrorDetails { get; set; } = "";

        public string GoogleStartAddress { get; set; } = "";
        public string GoogleEndAddress { get; set; } = "";

        public string BeginCoorindates => $"{BeginLatitude.ToString()}, {BeginLongitude.ToString()}";
        public string EndCoorindates => $"{EndLatitude.ToString()}, {EndLongitude.ToString()}";

        public bool IsEmptyBeginCoordinates => BeginLatitude == 0.0M && BeginLongitude == 0.0M;
    }
}
