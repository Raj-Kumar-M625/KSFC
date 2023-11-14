using KAR.KSFC.Components.Common.Logging.Enrichers;
using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;
using System.Linq;
using System.Security.Claims;

namespace KAR.KSFC.Components.Common.Logging.Enrichers
{
    public class UserClaimsEnricher : BaseEnricher
    {
        public const string Id = "userid";
        public const string Pan = "pan";
        private const string RPUserName = "UserName";
        private const string RPRoles = "Roles";
        public override void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var userName = GetUserName();
            var logEventPropertyuName = new LogEventProperty(RPUserName, new ScalarValue(userName));
            logEvent.AddOrUpdateProperty(logEventPropertyuName);

            var roles = GetRoles();
            var logEventPropertyRoles = new LogEventProperty(RPRoles, new ScalarValue(roles));
            logEvent.AddOrUpdateProperty(logEventPropertyRoles);
        }
        private string GetUserName()
        {
            //set employee user name
            var userName = ContextAccessor.HttpContext.User?.FindFirst(Id)?.Value;
            //set promoter pan as user name
            if (string.IsNullOrWhiteSpace(userName))
                userName = ContextAccessor.HttpContext.User?.FindFirst(Pan)?.Value;
            //set User Name as anonymous if user is not logged in
            if (string.IsNullOrWhiteSpace(userName))
                userName = "Anonymous";
            return userName;
        }
        private string GetRoles()
        {
            //set employee user name
            var roles = ContextAccessor.HttpContext.User?.FindAll(ClaimTypes.Role)?.Select(x => x.Value).ToList();
            //set User Name as anonymous if user is not logged in
            if (!roles.Any())
                roles.Add("Anonymous");
            return string.Join(";", roles);
        }

       
    }

    public class RequestHeadersEnricher : BaseEnricher
    {
        private const string RPRequestIdName = "RequestId";
        public override void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            string requestId = this.GetRequestId();
            if (string.IsNullOrWhiteSpace(requestId))
            {
                requestId = "unknown";
            }
            var logEventProperty = new LogEventProperty(RPRequestIdName, new ScalarValue(requestId));
            logEvent.AddPropertyIfAbsent(logEventProperty);
        }
        private string GetRequestId()
        {
            var reqId = ContextAccessor.HttpContext.Request.Headers[RPRequestIdName];
            return reqId.ToString();
        }
    }

    public class SessionEnricher : BaseEnricher
    {
        private const string SESSIONKEY_ADMINUSERNAME = "AdminUsername";
        private const string SESSIONKEY_USERNAME = "Username";
        private const string ANONYMOUS_USERNAME = "Anonymous";
        private const string PROPERTY_USERNAME = "User Name";
        public override void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var userName = GetUserName();
            var logEventPropertyuName = new LogEventProperty(PROPERTY_USERNAME, new ScalarValue(userName));
            logEvent.AddOrUpdateProperty(logEventPropertyuName);
        }
        private string GetUserName()
        {
            // set employee user name
            var userName = ContextAccessor.HttpContext?.Session?.GetString(SESSIONKEY_ADMINUSERNAME);
            //set promoter user name
            if (string.IsNullOrWhiteSpace(userName))
                userName = ContextAccessor.HttpContext?.Session?.GetString(SESSIONKEY_USERNAME);
            //set User Name as anonymous if user is not logged in
            if (string.IsNullOrWhiteSpace(userName))
                userName = ANONYMOUS_USERNAME;
            return userName;
        }
    }
}
