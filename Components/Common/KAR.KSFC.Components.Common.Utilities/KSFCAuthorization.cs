using KAR.KSFC.Components.Common.Utilities.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Utilities
{
   public class KSFCAuthorization : Attribute, IAsyncAuthorizationFilter
    {
        private string Role { get; set; }

        public KSFCAuthorization()
        {
        }

        public KSFCAuthorization(string role)
        {
            Role = role;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new JsonResult(new ApiResponse(401));
                await context.HttpContext.Response.WriteAsync(ResponseWriter(context, 401, "Identity not authenticated", "Identity not authenticated"));
            }
            else if (!string.IsNullOrEmpty(Role) && !context.HttpContext.User.IsInRole(Role))
            {
                context.Result = new JsonResult(new ApiException(403));
                await context.HttpContext.Response.WriteAsync(ResponseWriter(context, 403, "Permission restricted", "Forbidden"));
            }
        }

        /// <summary>
        /// ResponseWriter
        /// </summary>
        /// <param name="context"></param>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        private string ResponseWriter(AuthorizationFilterContext context, int statusCode, string message, string details)
        {
            dynamic response = string.Empty;
            context.HttpContext.Response.ContentType = "application/json";

            if (statusCode == 401)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                response = new ApiResponse((int)HttpStatusCode.Unauthorized);
            }
            else if (statusCode == 403)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                response = new ApiException((int)HttpStatusCode.Forbidden, "Permission restricted", "Forbidden");
            }

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return JsonSerializer.Serialize(response, options);
        }
    }
}
