using Application.DTOs.TDS;
using Domain.Payment;
using Presentation.Extensions.Bill;
using Presentation.Utils;
using System;
using System.Linq;

namespace Presentation.GridFilters.TDS
{
    public class TdsQuarterlyPaidListSearchFilters
    {
        /// <summary>
        /// Purpose = TDS Quarterly Filing Search criteria
        /// Author = Karthick J 
        /// Date = 08 10 2022   
        /// </summary>
        /// <param name="challanList"></param>
        /// <param name="tdsQuarterlyPaidListFilters"></param>
        /// <returns></returns>
        public FilterBuilder<TdssPaymentChallanDto> GetTDSQuarterlyPaidSearchCriteria(IQueryable<TdssPaymentChallanDto> challanList, TdsQuarterlyPaidListFilter tdsQuarterlyPaidListFilters)
        {
            DateTime startDate = default(DateTime);
            DateTime endDate = default(DateTime);
            DateTime dt;

            if (tdsQuarterlyPaidListFilters.challanDate != null)
            {

                var date = tdsQuarterlyPaidListFilters.challanDate.Substring(0, 10);
                var res = DateTime.TryParse(date, out dt);
                if (res)
                    startDate = DateTime.Parse(date);
                else
                    startDate = DateTime.Parse(tdsQuarterlyPaidListFilters.challanDate.Substring(0, 9));

                var dateend = tdsQuarterlyPaidListFilters.challanDate.Substring(11);
                var resend = DateTime.TryParse(dateend, out dt);
                if (resend)
                    endDate = DateTime.Parse(dateend);
                else
                    endDate = DateTime.Parse(tdsQuarterlyPaidListFilters.challanDate.Substring(12, 11));

            }





            tdsQuarterlyPaidListFilters.forder = tdsQuarterlyPaidListFilters.forder ?? new string[] { };

            //IQueryable<TdssPaymentChallanDto> Query = challanList.AsQueryable();
            var filterBuilder = new FilterBuilder<TdssPaymentChallanDto>();
            filterBuilder
                .Add("tdsQuarterlyPaidListFilters.tdsSection", q => tdsQuarterlyPaidListFilters.tdsSection != null ? q.Where(o => o.TDSPaymentChallan.TDSSection.Equals(tdsQuarterlyPaidListFilters.tdsSection)) : q)
                .Add("tdsQuarterlyPaidListFilters.noOfVendors", q => tdsQuarterlyPaidListFilters.noOfVendors > 0 ? q.Where(o => o.TDSPaymentChallan.NoOfVendors.Equals(tdsQuarterlyPaidListFilters.noOfVendors)) : q)
                .Add("tdsQuarterlyPaidListFilters.noOfTransactions", q => tdsQuarterlyPaidListFilters.noOfTransactions > 0 ? q.Where(o => o.TDSPaymentChallan.NoOfTrans.Equals(tdsQuarterlyPaidListFilters.noOfTransactions)) : q)
                .Add("tdsQuarterlyPaidListFilters.payableMinAmount", q => tdsQuarterlyPaidListFilters.payableMinAmount > 0 ? q.Where(o => o.TDSPaymentChallan.TotalTDSPayment >= tdsQuarterlyPaidListFilters.payableMinAmount) : q)
                .Add("tdsQuarterlyPaidListFilters.payableMaxAmount", q => tdsQuarterlyPaidListFilters.payableMaxAmount > 0 ? q.Where(o => o.TDSPaymentChallan.TotalTDSPayment <= tdsQuarterlyPaidListFilters.payableMaxAmount) : q)
                .Add("tdsQuarterlyPaidListFilters.bankName", q => tdsQuarterlyPaidListFilters.bankName != null ? q.Where(o => o.Bank != null && o.Bank.BankName.Equals(tdsQuarterlyPaidListFilters.bankName)) : q)
                .Add("tdsQuarterlyPaidListFilters.accountNo", q => tdsQuarterlyPaidListFilters.accountNo != null ? q.Where(o => o.Bank != null && o.Bank.Accountnumber.Equals(tdsQuarterlyPaidListFilters.accountNo)) : q)
                .Add("tdsQuarterlyPaidListFilters.quarter", q => !string.IsNullOrEmpty(tdsQuarterlyPaidListFilters.quarter) ? q.Where(o => o.TDSPaymentChallan.QuarterMaster.CodeName.Equals(tdsQuarterlyPaidListFilters.quarter)) : q)
                .Add("tdsQuarterlyPaidListFilters.assessmentYear", q => tdsQuarterlyPaidListFilters.assessmentYear > 0 ? q.Where(o => o.TDSPaymentChallan.AssementYear.Equals(tdsQuarterlyPaidListFilters.assessmentYear)) : q)
                .Add("tdsQuarterlyPaidListFilters.challanDate", q => tdsQuarterlyPaidListFilters.challanDate != null ? q.Where(o => o.TDSPaymentChallan.CreatedOn.Date >= startDate && o.TDSPaymentChallan.CreatedOn.Date <= endDate) : q);

            return filterBuilder;
        }
    }
}
