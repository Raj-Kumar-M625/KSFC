using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class EORPByMonth : MinimumResponse
    {
        public IEnumerable<EORPMonthResponse> EORPMonths { get; set; }
    }
}