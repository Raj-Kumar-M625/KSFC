using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class AreaCodeResponse : MinimumResponse
    {
        public IEnumerable<CodeTableEx> AreaCodes { get; set; }
    }
}