using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;
using System.Net;
using Newtonsoft.Json;
using System.Configuration;
using CRMUtilities;

namespace BusinessLayer
{
    // https://maps.googleapis.com/maps/api/distancematrix/json?origins=16.817483700,75.722502600&destinations=16.817483700,75.722502600
    internal class GoogleMaps : Maps
    {
        public override void CalculateDistanceInMeters(TrackingRecordForDistance record)
        {
            record.GoogleMapsDistanceInMeters = 0;

            using (WebClient wc = new WebClient())
            {
                string url = "https://maps.googleapis.com/maps/api/distancematrix/json";

                wc.QueryString.Clear();

                // if it is start of day, we just want to get address and hence
                // passing same coordinates for start and end
                if (record.IsEmptyBeginCoordinates)
                {
                    wc.QueryString.Add("origins", $"{record.EndCoorindates}");
                }
                else
                {
                    wc.QueryString.Add("origins", $"{record.BeginCoorindates}");
                }

                wc.QueryString.Add("destinations", $"{record.EndCoorindates}");
                wc.QueryString.Add("key", Utils.GoogleMapApiKey);

                string data = wc.DownloadString(url);

                // storing data - for case of exception
                record.GoogleMapErrorDetails = data;

                dynamic deserializedData = JsonConvert.DeserializeObject(data);
                string statusCode = deserializedData.rows[0].elements[0].status;

                if (statusCode.Equals("OK", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        decimal distanceInMeters = deserializedData.rows[0].elements[0].distance.value;
                        record.GoogleMapsDistanceInMeters = distanceInMeters;

                        // pickup start and end address
                        if (!record.IsEmptyBeginCoordinates)
                        {
                            record.GoogleStartAddress = Utils.TruncateString((string)deserializedData.origin_addresses[0], 128);
                        }
                        record.GoogleEndAddress = Utils.TruncateString((string)deserializedData.destination_addresses[0], 128);
                    }
                    catch(Exception)
                    {
                        record.GoogleMapsDistanceInMeters = 0;
                        record.GoogleStartAddress = record.BeginCoorindates;
                        record.GoogleEndAddress = record.EndCoorindates;
                    }
                }
                else
                {
                    record.GoogleMapsDistanceInMeters = -1;
                    record.GoogleMapError = true;
                    record.GoogleMapErrorDetails = data;

                    record.GoogleStartAddress = record.BeginCoorindates;
                    record.GoogleEndAddress = record.EndCoorindates;
                }
            }
        }
    }
}
