using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class ActivityContactDataModel
    {
        public long Id { get; set; }
        public long ActivityId { get; set; }
        public string Name { get; set; }
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
        [DisplayName("Is Primary")]
        public bool IsPrimary { get; set; }
    }
}