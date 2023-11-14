using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Models.Exception
{
    public class ProArgumentNullException : ArgumentNullException
    {
        public ProArgumentNullException(string paramName) : base(paramName)
        {
        }
    }
}
