using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class OfficeHierarchyModel
    {
        [Display(Name ="Zone Code")]
        public string ZoneCode { get; set; }
        [Display(Name = "Zone Name")]
        public string ZoneName { get; set; }
        [Display(Name = "Area Code")]
        public string AreaCode { get; set; }
        [Display(Name = "Area Name")]
        public string AreaName { get; set; }
        [Display(Name = "Territory Code")]
        public string TerritoryCode { get; set; }
        [Display(Name = "Territory Name")]
        public string TerritoryName { get; set; }
        [Display(Name = "HQ Code")]
        public string HQCode { get; set; }
        [Display(Name = "HQ Name")]
        public string HQName { get; set; }
    }

    public class SelectableOfficeHierarchyModel : OfficeHierarchyModel
    {
        [Display(Name = "Can Select Zone?")]
        public bool IsZoneSelectable { get; set; }

        [Display(Name = "Can Select Area?")]
        public bool IsAreaSelectable { get; set; }

        [Display(Name = "Can Select Territory?")]
        public bool IsTerritorySelectable { get; set; }

        [Display(Name = "Can Select HQ?")]
        public bool IsHQSelectable { get; set; }
    }
}