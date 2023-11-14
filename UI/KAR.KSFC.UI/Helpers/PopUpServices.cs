using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.IO;

namespace KAR.KSFC.UI.Helpers
{
    public static class PopUpServices
    {
        public static string Notify(string message, string title, Alerts notificationType = Alerts.success)
        {
            var msg = new
            {
                message = message,
                title = title,
                icon = notificationType.ToString(),
                type = notificationType.ToString(),
                provider = GetProvider()
            };

            return JsonConvert.SerializeObject(msg);
        }

        private static string GetProvider()
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();

            var value = configuration["NotificationProvider"];

            return value;
        }
    }
}
