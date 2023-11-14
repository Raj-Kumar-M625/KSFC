using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog.Context;

namespace KAR.KSFC.UI.Filters
{
    public class SetLogContext : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //set promoter user name
            var userName = context.HttpContext?.Session?.GetString("CustomerUsername");
            //set employee user name
            if(string.IsNullOrWhiteSpace(userName))
                userName = context.HttpContext?.Session?.GetString("AdminUsername");
            //set User Name as anonymous if user is not logged in
            if (string.IsNullOrWhiteSpace(userName))
                userName = "Anonymous";
            LogContext.PushProperty("UserName", userName);
            LogContext.PushProperty("ControllerName", context.RouteData.Values["controller"]);
            LogContext.PushProperty("ActionName", context.RouteData.Values["action"]);
            LogContext.PushProperty("AreaName", context.RouteData.Values["area"]);
        }
    }
}
