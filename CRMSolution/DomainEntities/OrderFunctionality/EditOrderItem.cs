using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class EditOrderItem
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public int RevisedQuantity { get; set; }
    }
}
