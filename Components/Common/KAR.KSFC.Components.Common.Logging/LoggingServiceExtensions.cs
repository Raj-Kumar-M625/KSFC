using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Logging.Enrichers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ILogger = KAR.KSFC.Components.Common.Logging.Client.ILogger;

namespace KAR.KSFC.Components.Common.Logging
{
    public static class LoggingServiceExtensions
    {
        /// <summary>
        /// Creating logging service with various Enrichers
        /// </summary>
        /// <param name="services"></param>
        /// <param name="envName"></param>
        /// <param name="enricherTypes"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterLoggingService(this IServiceCollection services, string envName, List<EnricherTypeEnum> enricherTypes = null)
        {
            envName = string.IsNullOrWhiteSpace(envName) ? "" : $"{envName}.";
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile($"logsettings.{envName}json", optional: true)
            .Build();

            services.AddHttpContextAccessor();
            var logConfig = new LoggerConfiguration()
                .Enrich.WithClientIp()
                .Enrich.WithClientAgent()
                .Enrich.FromLogContext()
                .Enrich.WithThreadId()
                .Enrich.WithProcessId()
                .Enrich.WithProcessName()
                .ReadFrom.Configuration(config);
            //apply enricher types passing from client
            if (enricherTypes!=null && enricherTypes.Any())
            {
                if (enricherTypes.Contains(EnricherTypeEnum.SessionPrams))
                    logConfig = logConfig.Enrich.WithSessionEnricher();
                if(enricherTypes.Contains(EnricherTypeEnum.RequestHeaders))
                    logConfig = logConfig.Enrich.WithRequestHeadersEnricher();
                if (enricherTypes.Contains(EnricherTypeEnum.UserClaims))
                    logConfig = logConfig.Enrich.WithUserClaimsEnricher();
            }
            var seriLogger = ((Serilog.ILogger)logConfig.CreateLogger());
            var logger = (ILogger)new Logger(seriLogger);
            services.AddSingleton(logger);
            return services;
        }
        private static LoggerConfiguration WithSessionEnricher(this LoggerEnrichmentConfiguration enrichmentConfiguration)
        {
            if (enrichmentConfiguration == null)
            {
                throw new ArgumentNullException("enrichmentConfiguration");
            }
            return enrichmentConfiguration.With<SessionEnricher>();
        }
        private static LoggerConfiguration WithUserClaimsEnricher(this LoggerEnrichmentConfiguration enrichmentConfiguration)
        {
            if (enrichmentConfiguration == null)
            {
                throw new ArgumentNullException("enrichmentConfiguration");
            }
            return enrichmentConfiguration.With<UserClaimsEnricher>();
        }
        private static LoggerConfiguration WithRequestHeadersEnricher(this LoggerEnrichmentConfiguration enrichmentConfiguration)
        {
            if (enrichmentConfiguration == null)
            {
                throw new ArgumentNullException("enrichmentConfiguration");
            }
            return enrichmentConfiguration.With<RequestHeadersEnricher>();
        }
    }
}
