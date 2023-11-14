namespace Presentation.Extensions.Vendor
{
    public class VendorFilter
    {
        public string vendorName { get; set; }
        public string gstin_Number { get; set; }
        public string pan_Number { get; set; }
        public string category { get; set; }
        public string status { get; set; }
        public decimal payableAmount { get; set; }
        public decimal payableMaxAmount { get; set; }
        public decimal pay { get; set; }
        public string[] forder { get; set; }
    }
}
