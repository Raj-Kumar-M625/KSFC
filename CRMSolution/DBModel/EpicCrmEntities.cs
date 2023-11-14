using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBModel
{
    public partial class EpicCrmEntities
    {
        public EpicCrmEntities(string efConnection)
                : base(efConnection)
        {
        }
    }
}
