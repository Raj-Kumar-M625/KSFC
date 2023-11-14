using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class EntityCrop
    {
        public long Id { get; set; }
        public long EntityId { get; set; }
        public string CropName { get; set; }
    }
}
