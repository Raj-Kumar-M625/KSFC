using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteEntityContactDisplayData
    {
        public long Id { get; set; }
        [DisplayName("Entity Id")]
        public long SqliteEntityId { get; set; }
        public string Name { get; set; }
        [DisplayName("Contact Number")]
        public string PhoneNumber { get; set; }
        [DisplayName("Primary Contact")]
        public bool IsPrimary { get; set; }
    }
}