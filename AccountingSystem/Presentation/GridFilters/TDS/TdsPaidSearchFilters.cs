using Domain.Payment;
using Domain.Vendor;
using Presentation.Extensions.Bill;
using Presentation.Extensions.Vendor;
using Presentation.Utils;
using System.Linq;

namespace Presentation.GridFilters.TDS
{
    ///<summary>
    /// Purpose = TDS Search criteria
    /// Author = Karthick J 
    /// Date = 22 08 2022  
    /// </summary>
    public class TdsPaidSearchFilters
    {
        ///<summary>
        /// Purpose = TDS Search criteria
        /// Author = Karthick J 
        /// Date = 22 08 2022  
        /// </summary>
        public FilterBuilder<BillTdsPayment> GetTDSPaidSearchCriteria(IQueryable<BillTdsPayment> billsList, TdsPaidFilter tdsPaidFilters)
        {

            tdsPaidFilters.forder = tdsPaidFilters.forder ?? new string[] { };

           // IQueryable<BillTdsPayment> Query = billsList.AsQueryable();
            var filterBuilder = new FilterBuilder<BillTdsPayment>();
            filterBuilder
                .Add("tdsPaidFilters.paymentReferenceNo", q => tdsPaidFilters.paymentReferenceNo != null ? q.Where(o => o.Bill != null && o.Bill.BillReferenceNo.Contains(tdsPaidFilters.paymentReferenceNo)) : q)
                .Add("tdsPaidFilters.vendorName", q => tdsPaidFilters.vendorName != null ? q.Where(o => o.Bill != null && o.Bill.Vendor != null && o.Bill.Vendor.Name.Contains(tdsPaidFilters.vendorName)) : q)
                .Add("tdsPaidFilters.payableAmount", q => tdsPaidFilters.payableMinAmount > 0 ? q.Where(o => o.TDSPaymentChallan != null && o.TDSPaymentChallan.TotalTDSPayment >= tdsPaidFilters.payableMinAmount) : q)
                .Add("tdsPaidFilters.payableMaxAmount", q => tdsPaidFilters.payableMaxAmount > 0 ? q.Where(o => o.TDSPaymentChallan != null && o.TDSPaymentChallan.TotalTDSPayment <= tdsPaidFilters.payableMaxAmount) : q)
                .Add("tdsPaidFilters.tdsSection", q => tdsPaidFilters.tdsSection != null ? q.Where(o => o.TDSPaymentChallan != null && o.TDSPaymentChallan.TDSSection.Equals(tdsPaidFilters.tdsSection)) : q)
                .Add("tdsPaidFilters.tdsStatus", q => tdsPaidFilters.tdsStatus != null ? q.Where(o => o.TDSPaymentChallan != null && o.Bill.TDSStatus.ToString().Equals(tdsPaidFilters.tdsStatus)) : q)
                .Add("tdsPaidFilters.fromDate", q => tdsPaidFilters.fromDate != System.DateTime.MinValue ? q.Where(o => o.TDSPaymentChallan != null && o.TDSPaymentChallan.PaymentDate >= tdsPaidFilters.fromDate) : q)
                .Add("tdsPaidFilters.toDate", q => tdsPaidFilters.toDate != System.DateTime.MinValue ? q.Where(o => o.TDSPaymentChallan != null && o.TDSPaymentChallan.PaymentDate <= tdsPaidFilters.toDate) : q)
                .Add("tdsPaidFilters.challanNo", q => tdsPaidFilters.challanNo != null ? q.Where(o => o.TDSPaymentChallan != null && o.TDSPaymentChallan.ChallanNo.Contains(tdsPaidFilters.challanNo)) : q)
                .Add("tdsPaidFilters.bsrCode", q => tdsPaidFilters.bsrCode != null ? q.Where(o => o.TDSPaymentChallan != null && o.TDSPaymentChallan.BSRCode.Contains(tdsPaidFilters.bsrCode)) : q)
                .Add("tdsPaidFilters.utrNo", q => tdsPaidFilters.utrNo != null ? q.Where(o => o.TDSPaymentChallan != null && o.TDSPaymentChallan.UTRNo.Contains(tdsPaidFilters.utrNo)) : q);

            return filterBuilder;


        }
    }
}
