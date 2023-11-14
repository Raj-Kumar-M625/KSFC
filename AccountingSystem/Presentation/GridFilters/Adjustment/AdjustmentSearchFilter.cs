using Domain.Adjustment;
using Presentation.Utils;
using System;
using System.Linq;

namespace Presentation.GridFilters.Adjustment
{
    public class AdjustmentSearchFilter
    {
        public FilterBuilder<Adjustments> GetAdjustmentSearchCriteria(IQueryable<Adjustments> adjustmentList, AdjustmentFilters adjustmentFilters)
        {

            DateTime startDate = default(DateTime);
            DateTime endDate = default(DateTime);
            DateTime dt;
            if (adjustmentFilters.Date != null)
            {

                var date = adjustmentFilters.Date.Substring(0, 10);
                var res = DateTime.TryParse(date, out dt);
                if (res)
                    startDate = DateTime.Parse(date);
                else
                    startDate = DateTime.Parse(adjustmentFilters.Date.Substring(0, 9));

                var dateend = adjustmentFilters.Date.Substring(11);
                var resend = DateTime.TryParse(dateend, out dt);
                if (resend)
                    endDate = DateTime.Parse(dateend);
                else
                    endDate = DateTime.Parse(adjustmentFilters.Date.Substring(12, 11));
            }



            adjustmentFilters.forder = adjustmentFilters.forder ?? new string[] { };

            var filterBuilder = new FilterBuilder<Adjustments>();
            filterBuilder
                .Add("adjustmentFilters.name", q => adjustmentFilters.name != null ? q.Where(o => o.Vendor.Name.Contains(adjustmentFilters.name)) : q)
                .Add("adjustmentFilters.AdjustmentReferenceNo", q => adjustmentFilters.AdjustmentReferenceNo != null ? q.Where(o => o.AdjustmentReferenceNo.Contains(adjustmentFilters.AdjustmentReferenceNo)) : q)
                .Add("adjustmentFilters.Date", q => q.Where(o => o.Date.Date >= startDate && o.Date.Date <= endDate))
                .Add("adjustmentFilters.approvedBy", q => adjustmentFilters.approvedBy == "Not Approved" ? q.Where(x => x.ApprovedBy == null) : q.Where(o => o.ApprovedBy.Contains(adjustmentFilters.approvedBy)))
                .Add("adjustmentFilters.createdBy", q => adjustmentFilters.createdBy != null ? q.Where(o => o.CreatedBy.Equals(adjustmentFilters.createdBy)) : q)
                 .Add("adjustmentFilters.Amount", q => q.Where(o => o.Amount >= adjustmentFilters.Amount))
             .Add("adjustmentFilters.Status", q => q.Where(o => o.AdjustmentStatus.StatusMaster.CodeValue.Equals(adjustmentFilters.Status)));
            return filterBuilder;


        }

    }
}
