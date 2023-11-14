using Domain.GSTTDS;
using Presentation.Extensions.Bill;
using Presentation.Utils;
using System;
using System.Linq;

namespace Presentation.GridFilters.GSTTDS
{
    /// <summary>
    /// Purpose = GST-TDS Paid List Search criteria
    /// Author = Karthick J 
    /// Date = 13 09 2022   
    /// </summary>
    public class GstTdsPaidSearchFilters
    {
        /// <summary>
        /// GST-TDS Paid List Search criteria
        /// </summary>
        /// <param name="billGSTTDSPaymentList"></param>
        /// <param name="gstTdsPaidFilters"></param>
        /// <returns></returns>
        public FilterBuilder<BillGsttdsPayment> GetGSTTDSPaidListSearchCriteria(IQueryable<BillGsttdsPayment> billGSTTDSPaymentList, GstTdsPaidFilter gstTdsPaidFilters)
        {


            DateTime startDate = default(DateTime);
            DateTime endDate = default(DateTime);
          
            DateTime dt;
            if (gstTdsPaidFilters.paymentFromDate != null)
            {

                var date = gstTdsPaidFilters.paymentFromDate.Substring(0, 10);
                var res = DateTime.TryParse(date, out dt);
                if (res)
                    startDate = DateTime.Parse(date);
                else
                    startDate = DateTime.Parse(gstTdsPaidFilters.paymentFromDate.Substring(0, 9));

                var dateend = gstTdsPaidFilters.paymentFromDate.Substring(11);
                var resend = DateTime.TryParse(dateend, out dt);
                if (resend)
                    endDate = DateTime.Parse(dateend);
                else
                    endDate = DateTime.Parse(gstTdsPaidFilters.paymentFromDate.Substring(12, 11));

            }






            gstTdsPaidFilters.forder ??= new string[] { };

           // IQueryable<BillGsttdsPayment> Query = billGSTTDSPaymentList.AsQueryable();
            var filterBuilder = new FilterBuilder<BillGsttdsPayment>();
            filterBuilder
                .Add("gstTdsPaidFilters.billReferenceNo", q => gstTdsPaidFilters.billReferenceNo != null ? q.Where(o => o.Bill.BillReferenceNo.Contains(gstTdsPaidFilters.billReferenceNo)) : q)
                .Add("gstTdsPaidFilters.vendorName", q => gstTdsPaidFilters.vendorName != null ? q.Where(o => o.Bill.Vendor.Name.Contains(gstTdsPaidFilters.vendorName)) : q)
                .Add("gstTdsPaidFilters.paymentFromDate", q => gstTdsPaidFilters.paymentFromDate != null ? q.Where(o => o.GSTTDSPaymentChallan.PaidDate >= startDate && o.GSTTDSPaymentChallan.PaidDate <= endDate) : q)
               // .Add("gstTdsPaidFilters.paymentToDate", q => gstTdsPaidFilters.paymentToDate != DateTime.MinValue ? q.Where(o => o.GSTTDSPaymentChallan.PaidDate <= gstTdsPaidFilters.paymentToDate) : q)
                .Add("gstTdsPaidFilters.payableMinAmount", q => gstTdsPaidFilters.payableMinAmount > 0 ? q.Where(o => o.GSTTDSPaymentChallan.TotalGSTTDSPayment >= gstTdsPaidFilters.payableMinAmount) : q)
                .Add("gstTdsPaidFilters.payableMaxAmount", q => gstTdsPaidFilters.payableMaxAmount > 0 ? q.Where(o => o.GSTTDSPaymentChallan.TotalGSTTDSPayment <= gstTdsPaidFilters.payableMaxAmount) : q)
                .Add("gstTdsPaidFilters.gstinNumber", q => gstTdsPaidFilters.gstinNumber != null ? q.Where(o => o.Bill.Vendor.GSTIN_Number.Contains(gstTdsPaidFilters.gstinNumber)) : q)
                .Add("gstTdsPaidFilters.gstCertificate", q => gstTdsPaidFilters.gstCertificate != null ? q.Where(o => o.GSTTDSPaymentChallan.GSTR7ACertificate.Contains(gstTdsPaidFilters.gstCertificate)) : q)
              .Add("gstTdsPaidFilters.gstTdsStatus", q => gstTdsPaidFilters.gstTdsStatus != null ? q.Where(o => o.GSTTDSPaymentChallan.GSTTDSStatus.StatusMaster.CodeValue.Contains(gstTdsPaidFilters.gstTdsStatus)) : q)
                .Add("gstTdsPaidFilters.utrNo", q => gstTdsPaidFilters.utrNo != null ? q.Where(o => o.GSTTDSPaymentChallan.UTRNo.Contains(gstTdsPaidFilters.utrNo)) : q);

            return filterBuilder;


        }
    }
}
