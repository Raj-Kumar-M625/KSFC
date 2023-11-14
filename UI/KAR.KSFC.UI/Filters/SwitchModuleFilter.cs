using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Filters
{

    public class SwitchModuleFilter : Attribute, IAuthorizationFilter
    {
        public string SwitchedModule;
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //set promoter user name
            var switchedModule = context.HttpContext?.Session?.GetString("SwitchedRole");

            if (!switchedModule.Equals(SwitchedModule))
            {                         
                context.Result = new RedirectToActionResult("Error", "Home", new { Area = "" });
            }
        }
    }
}
