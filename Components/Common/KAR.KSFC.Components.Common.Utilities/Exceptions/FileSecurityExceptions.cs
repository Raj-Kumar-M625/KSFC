using System;

namespace KAR.KSFC.Components.Common.Utilities.Exceptions
{
    [Serializable]
    public class FileSecurityExceptions: Exception
    {
        public FileSecurityExceptions(string message)
            : base(message)
        {
        }
        public FileSecurityExceptions(string message, Exception innerException)
            : base(message, innerException)
        {
        }
      
    }
}