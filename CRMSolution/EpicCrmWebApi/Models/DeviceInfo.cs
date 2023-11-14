using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class DeviceInfo
    {
        public string Model { get; set; }
        public string OSVersion { get; set; }
        public string AppVersion { get; set; }
        public string IMEI { get; set; }
    }
}