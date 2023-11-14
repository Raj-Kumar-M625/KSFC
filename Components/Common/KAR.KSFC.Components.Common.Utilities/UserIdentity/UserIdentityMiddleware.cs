
namespace KAR.KSFC.Components.Common.Utilities.UserIdentity
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public sealed class UserIdentityMiddleware
    {
        //Request Delegate for each Http request   
        private readonly RequestDelegate _requestDelegate;

        public UserIdentityMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        /// <summary>
        /// Set all User info
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                if (httpContext.RequestServices.GetService(typeof(UserInfo)) is UserInfo userInfo)
                {
                    userInfo.UserId = httpContext.User?.FindFirst(IndentityClaimsConstants.Id)?.Value;

                    userInfo.Email = httpContext.User?.FindFirst(ClaimTypes.Email)?.Value;

                    userInfo.Name = httpContext.User?.FindFirst(ClaimTypes.GivenName)?.Value;

                    userInfo.PhoneNumber = httpContext.User?.FindFirst(IndentityClaimsConstants.phoneNumber)?.Value;

                    userInfo.Pan = httpContext.User?.FindFirst(IndentityClaimsConstants.pan)?.Value;

                    userInfo.Role = httpContext.User?.FindAll(ClaimTypes.Role)?.Select(x => x.Value).ToList();
                    
                }
                await _requestDelegate(httpContext);
            } catch(Exception e)
            {
                throw (e);

            }
        }

        
    }

    public static class ConfigureUserInfoMiddleware
    {
        public static void CustomUserInfoMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<UserIdentityMiddleware>();
        }

        public static IServiceCollection AddUserInformation(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddScoped<UserInfo>();
        }
    }

   
}
