using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace KAR.KSFC.Components.Common.SeriLogging
{
    public static class SeriLogExtension
    {
        public static IServiceCollection RegisterSeriLogger(this IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
               //.WriteTo.Console() {Timestamp:yyyy-mm-dd}
               .WriteTo.File("Logs/log.txt", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning,
               rollingInterval: RollingInterval.Minute, retainedFileCountLimit: 2, rollOnFileSizeLimit: true, fileSizeLimitBytes: 55)

             //  .WriteTo.File("Logs/log1.txt", outputTemplate: " {MachineName} {ThreadId} {EnvironmentName} {Message} {Exception :1} {NewLine}")
               .Enrich.WithThreadId()
               .Enrich.WithMachineName()
               .Enrich.WithEnvironmentName()
               .CreateLogger();

            return services;
        }
    }
}
