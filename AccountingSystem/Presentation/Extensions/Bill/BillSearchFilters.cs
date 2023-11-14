using Domain.Bill;
using Presentation.Utils;
using System;
using System.Linq;

namespace Presentation.Extensions.Bill
{
    public class BillSearchFilters
    {
        public FilterBuilder<Bills> GetBillSearchCriteria(IQueryable<Bills> list, BillFilter billfilter)
        {
            DateTime startDate = default(DateTime);
            DateTime endDate = default(DateTime);
            DateTime DueStartDate = default(DateTime);
            DateTime DueEndDate = default(DateTime);
            DateTime dt;
            if (billfilter.BillDate != null)
            {

                var date = billfilter.BillDate.Substring(0, 10);
                var res = DateTime.TryParse(date, out dt);
                if (res)
                    startDate = DateTime.Parse(date);
                else
                    startDate = DateTime.Parse(billfilter.BillDate.Substring(0, 9));

                var dateend = billfilter.BillDate.Substring(11);
                var resend = DateTime.TryParse(dateend, out dt);
                if (resend)
                    endDate = DateTime.Parse(dateend);
                else
                    endDate = DateTime.Parse(billfilter.BillDate.Substring(12, 11));

            }
            if (billfilter.DueDate != null)
            {
                var date = billfilter.DueDate.Substring(0, 10);
                var res = DateTime.TryParse(date, out dt);
                if (res)
                    DueStartDate = DateTime.Parse(date);
                else
                    DueStartDate = DateTime.Parse(billfilter.DueDate.Substring(0, 9));

                var dateend = billfilter.DueDate.Substring(11);
                var resend = DateTime.TryParse(dateend, out dt);
                if (resend)
                    DueEndDate = DateTime.Parse(dateend);
                else
                    DueEndDate = DateTime.Parse(billfilter.DueDate.Substring(12, 11));
            }
            billfilter.forder = billfilter.forder ?? new string[] { };
            //IQueryable<Bills> Query = list.AsQueryable();
           // var frow = new BillFilterRow();
            var filterBuilder = new FilterBuilder<Bills>();
            filterBuilder
                .Add("billfilter.VendorName", q => billfilter.vendorName != null ? q.Where(o => o.Vendor.Name.Contains(billfilter.vendorName)) : q)
                .Add("billfilter.BillNumber", q => billfilter.BillNumber != null ? q.Where(o => o.BillReferenceNo.Equals(billfilter.BillNumber)) : q)
                .Add("billfilter.ReferenceNo", q => billfilter.ReferenceNo != null ? q.Where(o => o.BillReferenceNo.Equals(billfilter.ReferenceNo)) : q)
                .Add("billfilter.BillDate", q => billfilter.BillDate != null ? q.Where(o => o.BillDate >= startDate && o.BillDate <= endDate) : q)
                .Add("billfilter.DueDate", q => billfilter.DueDate != null ? q.Where(o => o.BillDueDate >= DueStartDate && o.BillDueDate <= DueEndDate) : q)
                .Add("billfilter.TotalPayableAmount", q => billfilter.TotalPayableAmount > 0 ? q.Where(o => o.NetPayable.Equals(billfilter.TotalPayableAmount)) : q)
                .Add("billfilter.BillTotal", q => billfilter.BillTotal > 0 ? q.Where(o => o.TotalBillAmount.Equals(billfilter.BillTotal)) : q)
                .Add("billfilter.Status", q => billfilter.Status != null ? q.Where(o => (o.BillStatus.StatusMaster.CodeValue).Equals(billfilter.Status)) : q)
                .Add("billfilter.Category", q => billfilter.Category != null ? q.Where(o => o.Vendor.VendorDefaults.Category == billfilter.Category) : q);
            return filterBuilder;

        }
    }
}
