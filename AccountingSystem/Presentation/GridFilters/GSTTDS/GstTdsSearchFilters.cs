using Domain.Bill;
using Presentation.Extensions.Bill;
using Presentation.GridFilters.GSTTDS;
using Presentation.Models.Bill;
using Presentation.Utils;
using System;
using System.Linq;

namespace Presentation.GridFilters.GSTTDS
{
    ///<summary>
    /// Purpose = GST-TDS Search criteria
    /// Author = Karthick J 
    /// Date = 12 09 2022  
    /// </summary>
    public class GstTdsSearchFilters
    {
        ///<summary>
        /// Purpose = TDS Search criteria
        /// Author = Karthick J 
        /// Date = 22 08 2022  
        /// </summary>
        public FilterBuilder<Bills> GetGSTTDSSearchCriteria(IQueryable<Bills> billsList, GstTdsFilter gsttdsFilters)
        {

            DateTime startDate = default(DateTime);
            DateTime endDate = default(DateTime);
            
            DateTime dt;
            if (gsttdsFilters.paymentFromDate != null)
            {

                var date = gsttdsFilters.paymentFromDate.Substring(0, 10);
                var res = DateTime.TryParse(date, out dt);
                if (res)
                    startDate = DateTime.Parse(date);
                else
                    startDate = DateTime.Parse(gsttdsFilters.paymentFromDate.Substring(0, 9));

                var dateend = gsttdsFilters.paymentFromDate.Substring(11);
                var resend = DateTime.TryParse(dateend, out dt);
                if (resend)
                    endDate = DateTime.Parse(dateend);
                else
                    endDate = DateTime.Parse(gsttdsFilters.paymentFromDate.Substring(12, 11));

            }




            gsttdsFilters.forder = gsttdsFilters.forder ?? new string[] { };

           // IQueryable<Bills> Query = billsList.AsQueryable();
            var filterBuilder = new FilterBuilder<Bills>();
            filterBuilder
                .Add("gsttdsFilters.billReferenceNo", q => gsttdsFilters.billReferenceNo != null ? q.Where(o => o.BillReferenceNo.Contains(gsttdsFilters.billReferenceNo)) : q)
                .Add("gsttdsFilters.vendorName", q => gsttdsFilters.vendorName != null ? q.Where(o => o.Vendor.Name.Contains(gsttdsFilters.vendorName)) : q)
                .Add("gsttdsFilters.payableMinAmount", q => gsttdsFilters.payableMinAmount > 0 ? q.Where(o => o.GSTTDS >= gsttdsFilters.payableMinAmount) : q)
                .Add("gsttdsFilters.payableMaxAmount", q => gsttdsFilters.payableMaxAmount > 0 ? q.Where(o => o.GSTTDS <= gsttdsFilters.payableMaxAmount) : q)
                .Add("gsttdsFilters.gstinNumber", q => gsttdsFilters.gstinNumber != null ? q.Where(o => o.Vendor.GSTIN_Number.Contains(gsttdsFilters.gstinNumber)) : q)
                .Add("gsttdsFilters.gstTdsStatus", q => gsttdsFilters.gstTdsStatus != null ? q.Where(o => (o.GSTTDSStatus.StatusMaster.CodeValue).Contains(gsttdsFilters.gstTdsStatus)) : q)
               .Add("gsttdsFilters.paymentFromDate", q => gsttdsFilters.paymentFromDate != null ? q.Where(o => o.BillDate >= startDate && o.BillDate <= endDate) : q);
                //.Add("gsttdsFilters.paymentToDate", q => gsttdsFilters.paymentToDate != System.DateTime.MinValue ? q.Where(o => o.BillDate <= gsttdsFilters.paymentToDate) : q);

            return filterBuilder;

        }
    }
}
