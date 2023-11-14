using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class DownloadTransporter
    {
        public long Id { get; set; }
        public string CompanyName { get; set; }
        public string VehicleType { get; set; }
        public string VehicleNo { get; set; }
        public int VehicleCapacityKgs { get; set; }
    }
}