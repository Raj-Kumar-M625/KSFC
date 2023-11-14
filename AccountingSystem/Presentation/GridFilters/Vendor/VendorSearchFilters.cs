using Domain.Vendor;
using Presentation.Utils;
using System.Linq;

namespace Presentation.Extensions.Vendor
{
    /// <summary>
    /// Purpose = Search filters class
    /// Author = Swetha M 
    /// Date = 29 06 2022  
    /// </summary>
    public class VendorSearchFilters
    {
        ///<summary>
        /// Purpose = Vendor Search criteria
        /// Author = Swetha M 
        /// Date = 29 06 2022  
        /// </summary>
        public FilterBuilder<Vendors> GetVendorSearchCriteria(IQueryable<Vendors> vendorList,VendorFilter vendorfilters)
        {

            vendorfilters.forder = vendorfilters.forder ?? new string[] { };

            //IQueryable<Vendors> Query = vendorList.AsQueryable();
            //var frow = new VendorFilterRow();
            var filterBuilder = new FilterBuilder<Vendors>();
            filterBuilder
                .Add("vendorfilters.vendorName",q => vendorfilters.vendorName != null ? q.Where(o => o.Name.Contains(vendorfilters.vendorName)) : q)
                .Add("vendorfilters.gstin_Number",q => vendorfilters.gstin_Number != null ? q.Where(o => o.GSTIN_Number.Contains(vendorfilters.gstin_Number)) : q)
                .Add("vendorfilters.pan_Number",q => vendorfilters.pan_Number != null ? q.Where(o => o.PAN_Number.Contains(vendorfilters.pan_Number)) : q)
                .Add("vendorfilters.category",q => vendorfilters.category != null ? q.Where(o => o.VendorDefaults.Category == vendorfilters.category) : q)
                .Add("vendorfilters.status",q => vendorfilters.status != null ? q.Where(o => (o.Status ? "Active" : "Inactive").Equals(vendorfilters.status)) : q)
                 .Add("vendorfilters.payableAmount",q => q.Where(o => o.VendorBalance.TotalNetPayable >= vendorfilters.payableAmount))
                 .Add("vendorfilters.payableMaxAmount",q => q.Where(o => o.VendorBalance.TotalNetPayable <= vendorfilters.payableMaxAmount))
                .Add("vendorfilters.pay",q => q.Where(o => o.VendorBalance.BalanceAmount >= vendorfilters.payableAmount && o.VendorBalance.BalanceAmount <= vendorfilters.payableMaxAmount));
            return filterBuilder;


        }
    }
}
