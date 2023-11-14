using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class ItemMaster
    {
        public long Id { get; set; }
        public string ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public string Category { get; set; }
        public string Unit { get; set; }
        public bool IsActive { get; set; }
        public string Classification { get; set; }
        public ICollection<ItemMasterTypeName> TypeNames { get; set; }
    }

    public class ItemMasterTypeName
    {
        public long ItemMasterId { get; set; }
        public string TypeName { get; set; }
        public string ItemType { get; set; }
        public decimal Rate { get; set; }
        public decimal ReturnRate { get; set; }
    }
}
