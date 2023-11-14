using Domain.Payment;

using Presentation.Utils;
using System;
using System.Linq;

namespace Presentation.Extensions.Payment
{
    /// <summary>
    /// Author:Swetha M Date:06/05/2022
    /// Purpose:Payment Search Filter
    /// </summary>
    /// <returns></returns>
    public class PaymentSearchFilter
    {
        /// <summary>
        /// Author:Swetha M Date:06/05/2022
        /// Purpose:Get Payment SearchCriteria and pass it to the controller
        /// </summary>
        /// <returns></returns>
        public FilterBuilder<Payments> GetPaymentSearchCriteria(IQueryable<Payments> paymentList, PaymentFilters paymentFilters)
        {

            DateTime startDate = default(DateTime);
            DateTime endDate = default(DateTime);
            DateTime dt;
            if (paymentFilters.paymentDate != null)
            {

                var date = paymentFilters.paymentDate.Substring(0, 10);
                var res = DateTime.TryParse(date, out dt);
                if (res)
                    startDate = DateTime.Parse(date);
                else
                    startDate = DateTime.Parse(paymentFilters.paymentDate.Substring(0, 9));

                var dateend = paymentFilters.paymentDate.Substring(11);
                var resend = DateTime.TryParse(dateend, out dt);
                if (resend)
                    endDate = DateTime.Parse(dateend);
                else
                    endDate = DateTime.Parse(paymentFilters.paymentDate.Substring(12, 11));
            }






            paymentFilters.forder = paymentFilters.forder ?? new string[] { };

            //IQueryable<Payments> Query = paymentList.AsQueryable();
            //var frow = new PaymentFilterRow();
            var filterBuilder = new FilterBuilder<Payments>();
            filterBuilder
                .Add("paymentFilters.name", q => paymentFilters.name != null ? q.Where(o => o.Vendor.Name.Contains(paymentFilters.name)) : q)
                .Add("paymentFilters.paymentReferenceNo", q => paymentFilters.paymentReferenceNo != null ? q.Where(o => o.PaymentReferenceNo.Contains(paymentFilters.paymentReferenceNo)) : q)
                .Add("paymentFilters.paymentDate", q => q.Where(o => o.PaymentDate.Date>= startDate && o.PaymentDate.Date <= endDate))
                .Add("paymentFilters.approvedBy", q => paymentFilters.approvedBy=="Not Approved" ? q.Where(x=>x.ApprovedBy== null) : q.Where(o => o.ApprovedBy.Contains(paymentFilters.approvedBy)))
                .Add("paymentFilters.createdBy", q => paymentFilters.createdBy != null ? q.Where(o => o.CreatedBy.Equals(paymentFilters.createdBy)) : q)
                 .Add("paymentFilters.payableAmount", q => q.Where(o => o.PaidAmount >= paymentFilters.payableAmount))
                 .Add("paymentFilters.payableMaxAmount", q => q.Where(o => o.PaidAmount <= paymentFilters.payableMaxAmount))
                .Add("paymentFilters.pay", q => q.Where(o => o.PaymentAmount >= paymentFilters.payableAmount && o.PaymentAmount <= paymentFilters.payableMaxAmount))
             //.Add("paymentFilters.paymentStatus", q => paymentFilters.paymentStatus != null ? q.Where(o => o.PaymentStatus.Equals(paymentFilters.paymentStatus)) : q);
             .Add("paymentFilters.paymentStatus",q=> q.Where(o => o.PaymentStatus.StatusMaster.CodeValue.Equals(paymentFilters.paymentStatus)))
             .Add("paymentFilters.type",q=> q.Where(o => o.Type.Equals(paymentFilters.type)));
             //.Add("paymentFilters.tdsStatus", q => paymentFilters.paymentStatus != null ? q.Where(o => o.PaymentStatus != null && o.PaymentStatus.StatusMaster != null && o.PaymentStatus.StatusMaster.CodeName.Equals(paymentFilters.paymentStatus)) : q);
            return filterBuilder;


        }
    }
}
