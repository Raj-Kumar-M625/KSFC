using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class EntityContactModel
    {
        public long Id { get; set; }
        public long EntityId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsPrimary { get; set; }
    }
}