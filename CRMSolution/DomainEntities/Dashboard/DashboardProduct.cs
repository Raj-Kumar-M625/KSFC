using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class DashboardProduct
    {
        public int Id { get; set; }
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public string UOM { get; set; }
        public string BrandName { get; set; }
        public int ShelfLifeInMonths { get; set; }
        public bool IsActive { get; set; }
        public string GroupName { get; set; }
        public string GstCode { get; set; }
        public ICollection<DashboardProductPrice> Prices { get; set; }
    }

    public class DashboardProductPrice : ProductPrice2
    {
        public string AreaCode { get; set; }
    }
}
