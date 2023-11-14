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
    internal class LinearMaps : Maps
    {
        public override void CalculateDistanceInMeters(TrackingRecordForDistance record)
        {
            record.LinearDistanceInMeters = (decimal)HaversineInM((double)record.BeginLatitude, (double)record.BeginLongitude, (double)record.EndLatitude, (double)record.EndLongitude);
        }

        private static double _eQuatorialEarthRadius = 6378.1370D;
        private static double _d2r = (Math.PI / 180D);

        private static double HaversineInM(double lat1, double long1, double lat2, double long2)
        {
            return (double)(1000D * HaversineInKM(lat1, long1, lat2, long2));
        }

        private static double HaversineInKM(double lat1, double long1, double lat2, double long2)
        {
            double dlong = (long2 - long1) * _d2r;
            double dlat = (lat2 - lat1) * _d2r;
            double a = Math.Pow(Math.Sin(dlat / 2D), 2D) + Math.Cos(lat1 * _d2r) * Math.Cos(lat2 * _d2r) * Math.Pow(Math.Sin(dlong / 2D), 2D);
            double c = 2D * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1D - a));
            double d = _eQuatorialEarthRadius * c;

            return d;
        }
    }
}
