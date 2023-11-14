using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicCrmWebApi
{
    public class ActivityRecordModel
    {
        public long Id { get; set; }
        public long EmployeeDayId { get; set; }

        [Display(Name = "Name")]
        public string ClientName { get; set; }

        [Display(Name = "Phone")]
        public string ClientPhone { get; set; }

        [Display(Name = "Type")]
        public string ClientType { get; set; }

        [Display(Name = "Activity")]
        public string ActivityType { get; set; }

        [Display(Name = "Comments")]
        public string Comments { get; set; }

        public System.DateTime At { get; set; }

        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
