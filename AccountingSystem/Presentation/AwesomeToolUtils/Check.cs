using Presentation.Models;
using Presentation.Models.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Utils
{
    public static class Check
    {
        public static void NotNull(object o, string name)
        {
            if (o == null)
            {
                throw new ProArgumentNullException(name);
            }
        }

        public static void IdHasVal(int o)
        {
            if (o == 0)
            {
                throw new ProArgumentNullException("id");
            }
        }
    }
}
