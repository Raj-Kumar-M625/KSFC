using Domain.Payment;
using Presentation.Utils;
using System;
using System.Linq;

namespace Presentation.GridFilters.TDS
{
    /// <summary>
    /// Purpose = TDS Quarterly Filed Search Criteria
    /// Author = Karthick J
    /// Date = 19 09 2022
    /// </summary>
    public class TdsQuarterlyFiledSearchFilters
    {
        /// <summary>
        /// Purpose = TDS Quarterly Filed Search criteria
        /// Author = Karthick J 
        /// Date = 19 09 2022   
        /// </summary>
        /// <param name="challanList"></param>
        /// <param name="tdsQuarterlyFiledFilters"></param>
        /// <returns></returns>
        public FilterBuilder<QuarterlyTdsPaymentChallan> GetTDSQuarterlyFiledSearchCriteria(IQueryable<QuarterlyTdsPaymentChallan> challanList, TdsQuarterlyFiledFilter tdsQuarterlyFiledFilters)
        {

            tdsQuarterlyFiledFilters.forder = tdsQuarterlyFiledFilters.forder ?? new string[] { };

            //IQueryable<QuarterlyTdsPaymentChallan> Query = challanList.AsQueryable();
            var filterBuilder = new FilterBuilder<QuarterlyTdsPaymentChallan>();
            filterBuilder
                .Add("tdsQuarterlyFiledFilters.noOfChallan", q => tdsQuarterlyFiledFilters.noOfChallan > 0 ? q.Where(o => o.NoOfChallan.Equals(tdsQuarterlyFiledFilters.noOfChallan)) : q)
                .Add("tdsQuarterlyFiledFilters.payableMinAmount", q => tdsQuarterlyFiledFilters.payableMinAmount > 0 ? q.Where(o => o.TotalAmount >= tdsQuarterlyFiledFilters.payableMinAmount) : q)
                .Add("tdsQuarterlyFiledFilters.payableMaxAmount", q => tdsQuarterlyFiledFilters.payableMaxAmount > 0 ? q.Where(o => o.TotalAmount <= tdsQuarterlyFiledFilters.payableMaxAmount) : q)
                .Add("tdsQuarterlyFiledFilters.quarter", q => !string.IsNullOrEmpty(tdsQuarterlyFiledFilters.quarter) ? q.Where(o => o.QuarterMaster.CodeName.Equals(tdsQuarterlyFiledFilters.quarter)) : q)
                .Add("tdsQuarterlyFiledFilters.assessmentYear", q => tdsQuarterlyFiledFilters.assessmentYear > 0 ? q.Where(o => o.AssementYear.Equals(tdsQuarterlyFiledFilters.assessmentYear)) : q);

            return filterBuilder;
        }
    }
}
