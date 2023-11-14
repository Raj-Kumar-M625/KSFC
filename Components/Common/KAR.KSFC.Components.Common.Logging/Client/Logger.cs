using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace KAR.KSFC.Components.Common.Logging.Client
{
    public class Logger : ILogger
    {
        public readonly Serilog.ILogger _logger;
        public Logger(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        public void Debug(string messageTemplate)
        {
            _logger.Debug(messageTemplate);
        }

        public void Error(string messageTemplate)
        {
            _logger.Error(messageTemplate);
        }

        public void Fatal(string messageTemplate)
        {
            _logger.Fatal(messageTemplate);
        }

        public void Information(string messageTemplate)
        {
            _logger.Information(messageTemplate);
        }

        public void Verbose(string messageTemplate)
        {
            _logger.Verbose(messageTemplate);
        }

        public void Warning(string messageTemplate)
        {
            _logger.Warning(messageTemplate);
        }
    }
}
