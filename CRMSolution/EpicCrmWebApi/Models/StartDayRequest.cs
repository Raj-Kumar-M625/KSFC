using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class StartDayRequest: RequestBase
    {
        public string Id { get; set; }
        [Required]
        public long EmployeeId { get; set; }

        //public long UserId { get; set; }

        [Required]
        public decimal Latitude { get; set; }

        [Required]
        public decimal Longitude { get; set; }

        public string IMEI { get; set; }

        public long MNC { get; set; }
        public long MCC { get; set; }
        public long LAC { get; set; }
        public long CellId { get; set; }

        public DeviceInfo DeviceInfo { get; set; }

        public int ActivityTrackingType { get; set; }
        public string ActivityType { get; set; }
    }
}