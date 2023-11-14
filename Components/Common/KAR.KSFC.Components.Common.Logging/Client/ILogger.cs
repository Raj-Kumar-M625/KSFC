using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Logging.Client
{
    public interface ILogger
    {
        void Information(string messageTemplate);
        void Warning(string messageTemplate);
        void Error(string messageTemplate);
        void Fatal(string messageTemplate);
        void Debug(string messageTemplate);
        void Verbose(string messageTemplate);
    }

}
