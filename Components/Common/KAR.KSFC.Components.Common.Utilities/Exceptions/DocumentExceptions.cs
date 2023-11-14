using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Utilities.Exceptions
{
    [Serializable]
    public class DocumentExceptions : Exception
    {
        public DocumentExceptions(string message)
            : base(message)
        {
        }
        public DocumentExceptions(string message, Exception innerException)
            : base(message, innerException)
        {
        }

    }
}
