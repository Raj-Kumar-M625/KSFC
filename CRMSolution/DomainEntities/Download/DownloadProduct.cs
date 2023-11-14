using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class DownloadProduct
    {
        public string GroupName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string UOM { get; set; }
        public decimal MRP { get; set; } //price per unit
        public bool IsActive { get; set; }
        public long Stock { get; set; }
        public string GstCode { get; set; }
        public decimal GstPercent { get; set; }

        public IEnumerable<DownloadProductPrice> PriceList { get; set; }
    }

    public class DownloadProductEx : DownloadProduct
    {
        public string AreaCode { get; set; }
    }

    // used in compress download, to support large volume of data;
    public class Product2
    {
        public long Id { get; set; }
        public string GroupName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string UOM { get; set; }
        public string GstCode { get; set; }
        public decimal GstPercent { get; set; }
    }

    public class ProductPrice2
    {
        public long Id { get; set; }
        public int ProductId { get; set; }
        public long Stock { get; set; }
        public decimal MRP { get; set; }
        public decimal DISTPrice { get; set; }
        public decimal PDISTPrice { get; set; }
        public decimal DEALERPrice { get; set; }
    }
}
