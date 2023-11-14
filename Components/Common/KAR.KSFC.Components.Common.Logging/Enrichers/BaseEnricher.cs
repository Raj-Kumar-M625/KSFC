using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Logging.Enrichers
{
    /// <summary>
    /// BaseEnricher for including additional user/request params to the serilog
    /// </summary>
    public abstract class BaseEnricher : ILogEventEnricher
    {
        public IHttpContextAccessor ContextAccessor;
        public BaseEnricher()
        {
            ContextAccessor = new HttpContextAccessor();
        }
        internal BaseEnricher(IHttpContextAccessor contextAccessor)
        {
            ContextAccessor = contextAccessor;
        }
        public abstract void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory);
    }
}
