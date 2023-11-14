using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class DBServer 
    {
        public string IP { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IntegratedAuth { get; set; }
    }
}
