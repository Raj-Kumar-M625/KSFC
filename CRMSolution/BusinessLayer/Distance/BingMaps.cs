using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;
using System.Net;
using Newtonsoft.Json;

namespace BusinessLayer
{
    internal class BingMaps : Maps
    {
        public override void CalculateDistanceInMeters(TrackingRecordForDistance record)
        {
            record.BingMapsDistanceInMeters = 0;

            using (WebClient wc = new WebClient())
            {
                string url = "http://dev.virtualearth.net/REST/V1/Routes/Driving";

                wc.QueryString.Clear();
                wc.QueryString.Add("wp.0", $"{record.BeginCoorindates}");
                wc.QueryString.Add("wp.1", $"{record.EndCoorindates}");
                wc.QueryString.Add("key", "AtjN9ZpdqBhKmBjvMCHq8ZlHiI-O5RkKZksWv3FTgPbUDTlaM6o1qxisCBTrWPTt");

                string data = wc.DownloadString(url);
                // storing data - for case of exception
                record.BingMapErrorDetails = data;

                dynamic deserializedData = JsonConvert.DeserializeObject(data);
                decimal distanceInKilometers = deserializedData.resourceSets[0].resources[0].travelDistance;
                string distanceUnit = deserializedData.resourceSets[0].resources[0].distanceUnit;
                int statusCode = deserializedData.statusCode;

                if (statusCode == 200)
                {
                    if( distanceUnit.Equals("Kilometer", StringComparison.OrdinalIgnoreCase))
                    {
                        record.BingMapsDistanceInMeters = distanceInKilometers * 1000;
                    }
                    else
                    {
                        record.BingMapsDistanceInMeters = distanceInKilometers;
                    }
                }
                else
                {
                    record.BingMapsDistanceInMeters = -1;
                    record.BingMapError = true;
                    record.BingMapErrorDetails = data;
                }
            }
        }
    }
}
