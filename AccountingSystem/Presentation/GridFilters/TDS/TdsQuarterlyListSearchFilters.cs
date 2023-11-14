using Domain.Payment;
using Presentation.Utils;
using System.Linq;

namespace Presentation.GridFilters.TDS
{
    public class TdsQuarterlyListSearchFilters
    {
        /// <summary>
        /// Purpose = TDS Quarterly List Search criteria
        /// Author = Karthick J 
        /// Date = 13 10 2022   
        /// </summary>
        /// <param name="challanList"></param>
        /// <param name="tdsQuarterlyListFilters"></param>
        /// <returns></returns>
        public FilterBuilder<QuarterlyTdsPaymentChallan> GetTDSQuarterlyListSearchCriteria(IQueryable<QuarterlyTdsPaymentChallan> challanList, TdsQuarterlyListFilter tdsQuarterlyListFilters)
        {

            tdsQuarterlyListFilters.forder = tdsQuarterlyListFilters.forder ?? new string[] { };

           // IQueryable<QuarterlyTdsPaymentChallan> Query = challanList.AsQueryable();
            var filterBuilder = new FilterBuilder<QuarterlyTdsPaymentChallan>();
            filterBuilder
                .Add("tdsQuarterlyListFilters.noOfChallan", q => tdsQuarterlyListFilters.noOfChallan > 0 ? q.Where(o => o.NoOfChallan.Equals(tdsQuarterlyListFilters.noOfChallan)) : q)
                .Add("tdsQuarterlyListFilters.payableMinAmount", q => tdsQuarterlyListFilters.payableMinAmount > 0 ? q.Where(o => o.TotalAmount >= tdsQuarterlyListFilters.payableMinAmount) : q)
                .Add("tdsQuarterlyListFilters.payableMaxAmount", q => tdsQuarterlyListFilters.payableMaxAmount > 0 ? q.Where(o => o.TotalAmount <= tdsQuarterlyListFilters.payableMaxAmount) : q)
                .Add("tdsQuarterlyListFilters.quarter", q => !string.IsNullOrEmpty(tdsQuarterlyListFilters.quarter) ? q.Where(o => o.QuarterMaster.CodeName.Equals(tdsQuarterlyListFilters.quarter)) : q)
                .Add("tdsQuarterlyListFilters.assessmentYear", q => tdsQuarterlyListFilters.assessmentYear > 0 ? q.Where(o => o.AssementYear.Equals(tdsQuarterlyListFilters.assessmentYear)) : q);

            return filterBuilder;
        }
    }
}
