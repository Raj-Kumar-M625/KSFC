using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Security
{
    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configure;

        public BasicAuthMiddleware(RequestDelegate next, IConfiguration configure)
        {
            _next = next;
            _configure = configure;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                var username = httpContext.Request.Headers["Username"].ToString();
                var password = httpContext.Request.Headers["Password"].ToString();

                if ((username != null && password != null) || (username != string.Empty && password != string.Empty))
                {
                    Encoding encoding = Encoding.GetEncoding("UTF-8");
                    string DecryPassword = Decrypt(password); //Encrpted pass is awBzAGYAYwBAADEAMgAzAA==

                    string SysUsername = _configure["BasicAuth:Username"];
                    string SysPassword = _configure["BasicAuth:Password"];

                    if (username == SysUsername && DecryPassword == SysPassword)
                    {
                        // _logger.Information("Authorization Successful with Caller UserName = " + username + " and Request Id = " + requestId);
                        await _next(httpContext);
                    }
                    else
                    {
                        //_logger.Information("Authorization failed for Caller UserName = " + username + " and Request Id = " + requestId);
                        httpContext.Response.StatusCode = 401;
                        await httpContext.Response.WriteAsync("Unauthorized credentials.");
                        return;
                    }
                }
                else
                {
                    if (username == null || password == null)
                    {
                        //_logger.Warning("Caller with UserName = " + username + ".  has requested without Authorization headers.");
                        httpContext.Response.StatusCode = 401;
                        await httpContext.Response.WriteAsync("Unauthorized request.");
                        return;
                    }

                    // _logger.Warning("Client with UserName = " + username + ", has requested without RequestId header.");
                    httpContext.Response.StatusCode = 401;
                    await httpContext.Response.WriteAsync("Unauthorized RequestId");
                    return;
                }
            }
            catch (Exception ex)
            {
                // _logger.Error("Authorization failed for Requested caller UserName = " + username + " . and request Id = " + requestId + Environment.NewLine + " Error occured at Basic Auth Middleware with Error message = " +
                //ex.Message + ". Inner exception = " + ex.InnerException + " . Stack trace = " + ex.StackTrace + Environment.NewLine);
                await httpContext.Response.WriteAsync(ex.StackTrace + ex.Message + ex.InnerException);//CustomExceptionVerbiages.E09);
                return;
            }
        }

        //public static string EnCrypt(string text)
        //{
        //    return Convert.ToBase64String(Encoding.Unicode.GetBytes(text));
        //}

        public static string Decrypt(string text)
        {
            return Encoding.Unicode.GetString(Convert.FromBase64String(text));
        }
    }

    public static class BasicAuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseBasicAuthMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BasicAuthMiddleware>();
        }
    }
}
