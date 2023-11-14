using KAR.KSFC.Components.Common.Utilities.UserIdentity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog.Context;

namespace KAR.KSFC.API.Filters
{
    public class SetLogContext : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            LogContext.PushProperty("ControllerName", context.RouteData.Values["controller"]);
            LogContext.PushProperty("ActionName", context.RouteData.Values["action"]);
            LogContext.PushProperty("AreaName", context.RouteData.Values["area"]);
        }
    }
}
