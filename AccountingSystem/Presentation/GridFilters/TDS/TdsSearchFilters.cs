using Application.DTOs.Bill;
using Domain.Bill;
using Domain.Vendor;
using Presentation.Extensions.Bill;
using Presentation.Extensions.Vendor;
using Presentation.Models.Bill;
using Presentation.Utils;
using System;
using System.Linq;

namespace Presentation.GridFilters.TDS
{
    ///<summary>
    /// Purpose = TDS Search criteria
    /// Author = Karthick J 
    /// Date = 22 08 2022  
    /// </summary>
    public class TdsSearchFilters
    {
        ///<summary>
        /// Purpose = TDS Search criteria
        /// Author = Karthick J 
        /// Date = 22 08 2022  
        /// </summary>
        public FilterBuilder<Bills> GetTDSSearchCriteria(IQueryable<Bills> billsList, TdsFilter tdsfilters)
        {
            DateTime startDate = default(DateTime);
            DateTime endDate = default(DateTime);
        
            DateTime dt;
            if (tdsfilters.paymentFromDate != null)
            {

                var date = tdsfilters.paymentFromDate.Substring(0, 10);
                var res = DateTime.TryParse(date, out dt);
                if (res)
                    startDate = DateTime.Parse(date);
                else
                    startDate = DateTime.Parse(tdsfilters.paymentFromDate.Substring(0, 9));

                var dateend = tdsfilters.paymentFromDate.Substring(11);
                var resend = DateTime.TryParse(dateend, out dt);
                if (resend)
                    endDate = DateTime.Parse(dateend);
                else
                    endDate = DateTime.Parse(tdsfilters.paymentFromDate.Substring(12, 11));

            }



            tdsfilters.forder = tdsfilters.forder ?? new string[] { };

           // IQueryable<Bills> Query = billsList.AsQueryable();
            var filterBuilder = new FilterBuilder<Bills>();
            filterBuilder
                .Add("tdsFilters.paymentReferenceNo", q => tdsfilters.paymentReferenceNo != null ? q.Where(o => o.BillReferenceNo.Contains(tdsfilters.paymentReferenceNo)) : q)
                .Add("tdsFilters.vendorName", q => tdsfilters.vendorName != null ? q.Where(o => o.Vendor.Name.Contains(tdsfilters.vendorName)) : q)
                .Add("tdsFilters.payableAmount", q => tdsfilters.payableAmount > 0 ? q.Where(o => o.TDS >= tdsfilters.payableAmount) : q)
                .Add("tdsFilters.payableMaxAmount", q => tdsfilters.payableMaxAmount > 0 ? q.Where(o => o.TDS <= tdsfilters.payableMaxAmount) : q)
                .Add("tdsFilters.tdsSection", q => tdsfilters.tdsSection != null ? q.Where(o => o.Vendor.VendorDefaults.TDSSection.Equals(tdsfilters.tdsSection)) : q)
                .Add("tdsFilters.tdsStatus", q => tdsfilters.tdsStatus != null ? q.Where(o => o.TDSStatus != null && o.TDSStatus.StatusMaster != null && o.TDSStatus.StatusMaster.Id.ToString().Equals(tdsfilters.tdsStatus)) : q)
                .Add("tdsFilters.paymentFromDate", q => tdsfilters.paymentFromDate !=null? q.Where(o => o.BillDate >= startDate && o.BillDate <= endDate) : q)
               // .Add("tdsFilters.paymentToDate", q => tdsfilters.paymentToDate != System.DateTime.MinValue ? q.Where(o => o.BillDate <= tdsfilters.paymentToDate) : q)
                .Add("tdsFilters.assessmentYear", q => tdsfilters.assessmentYear > 0 ? q.Where(o => o.AssementYearCMID.Equals(tdsfilters.assessmentYear)) : q);

            return filterBuilder;


        }
    }
}
