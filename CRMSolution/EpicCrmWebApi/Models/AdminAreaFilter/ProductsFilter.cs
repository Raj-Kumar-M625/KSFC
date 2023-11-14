using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class ProductsFilter
    {
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public string Zone { get; set; }
        public string Area { get; set; }

        public int MaxItems { get; set; }
    }
}