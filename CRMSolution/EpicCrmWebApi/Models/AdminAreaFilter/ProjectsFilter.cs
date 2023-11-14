using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class ProjectsFilter
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public int IsActive { get; set; }
    }
}