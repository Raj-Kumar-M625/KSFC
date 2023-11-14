using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Presentation.Helpers
{
    public class SessionTimeoutHandlerAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // If session exists
            if (filterContext.HttpContext.Session != null)
            {
                //if new session
                if (filterContext.HttpContext.Session.IsAvailable)
                {
                    var cookie = filterContext.HttpContext.Session.Keys.Where(x=>x== "_role").FirstOrDefault();
                    //if cookie exists and sessionid index is greater than zero
                    if (cookie == null)
                    {
                        //redirect to desired session
                        //expiration action and controller
                        filterContext.Result = new RedirectResult("~/Identity/Index"); 
                        return;
                    }
                }
            }
            //otherwise continue with action
            base.OnActionExecuting(filterContext);
        }
    }

}
